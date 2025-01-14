#pragma once
#include <initializer_list>
#include <functional>
#include <stdexcept>
#include <exception>

/*

	模板定义部分

*/



template <typename T>
struct LinkNode
{
	LinkNode<T>* next;
	LinkNode(LinkNode<T>* next = nullptr) noexcept : next(next)
	{
	}
};

template <typename T>
struct LinkValueNode : public LinkNode<T>
{
	T value;
	LinkValueNode(T value, LinkNode<T>* next = nullptr) noexcept :value(value), LinkNode<T>(next)
	{
	}
};


template<typename T>
class LinkedList
{
public:
	/// <summary>
	/// 筛选函数类型
	/// </summary>
	typedef std::function<bool(const T& data)> Where;
	/// <summary>
	/// 创建一个空的链表
	/// </summary>
	LinkedList() noexcept;
	/// <summary>
	/// 基于现有数据创建链表
	/// </summary>
	LinkedList(std::initializer_list<T> list) noexcept;
	LinkValueNode<T>* begin() const noexcept;
	/// <summary>
	/// 在链表指定索引位置插入数据
	/// 若索引超出长度，则操作将无效
	/// </summary>
	void insert(T data, size_t index) noexcept;
	/// <summary>
	/// 在链表最后插入数据
	/// </summary>
	/// <param name="data"></param>
	void insertLast(T data) noexcept;
	/// <summary>
	/// 在链表中删除满足条件的数据 返回操作执行次数
	/// </summary>
	size_t remove(Where where) noexcept;
	/// <summary>
	/// 删除链表中指定索引的数据
	/// 若索引超出长度，则操作将无效
	/// </summary>
	void removeAt(size_t index) noexcept;
	/// <summary>
	/// 在链表中更新满足条件的数据 返回操作执行次数
	/// </summary>
	size_t update(Where where, std::function<void(T& oldData)> updateSet) noexcept;
	/// <summary>
	/// 更新链表中指定索引的数据
	/// 若索引超出长度，则操作将无效
	/// </summary>
	void updateAt(size_t index, T newData) noexcept;


	/// <summary>
	/// 在链表中筛选满足条件的数据，并通过回调函数获取数据 返回操作执行次数
	/// </summary>
	size_t select(Where where, std::function<void(const T& data)> result) const noexcept;
	/// <summary>
	/// 获取链表中指定索引的数据
	/// 若索引超出长度则会抛出异常
	/// </summary>
	const T& selectAt(size_t index) const noexcept(false);

	size_t count() const noexcept;
	size_t count(Where where) const noexcept;

	void clear() noexcept;

	~LinkedList() noexcept;
private:
	LinkNode<T>* head = nullptr;
	size_t size = 0;
};

/*

	模板的实现部分
	这部分无法单独写到一个文件里，因为这样会产生编译错误

*/

#pragma region TemplateImpl

#pragma region Helpers

/// <summary>
/// 在 prev 节点和 next 节点（可为 nullptr ）之间插入一个节点 携带数据为 data
/// </summary>
template <typename T>
inline static void insertData(LinkNode<T>*& prev, T data, LinkNode<T>*& next)
{
	prev->next = new LinkValueNode<T>(data, next);
}

/// <summary>
/// 删除 cur 节点，并释放内存
/// </summary>
template <typename T>
inline static void removeData(LinkNode<T>*& prev, LinkNode<T>*& cur)
{
	prev->next = cur->next;
	delete cur;
	cur = nullptr;
}
#pragma endregion




template <typename T>
inline LinkedList<T>::LinkedList() noexcept : LinkedList({}) {}


template <typename T>
LinkedList<T>::LinkedList(std::initializer_list<T> list) noexcept
{
	// 链表的头不携带任何数据
	head = new LinkNode<T>();
	size = list.size();
	auto cur = head;

	for (auto& data : list)
	{
		// 链表中间携带数据 cur->next 一直为 nullptr
		insertData(cur, data, cur->next);
		cur = cur->next;
	}

}

template<typename T>
inline LinkValueNode<T>* LinkedList<T>::begin() const noexcept
{
	return static_cast<LinkValueNode<T>*>(head->next);
}

template <typename T>
void LinkedList<T>::insert(T data, size_t index) noexcept
{
	if (index > size)
		return;
	auto prev = head;
	auto next = head->next;

	while (index)
	{
		prev = next;
		next = prev->next;
		index--;
	}

	insertData(prev, data, next);
	size++;
}

template<typename T>
void LinkedList<T>::insertLast(T data) noexcept
{
	insert(data, size);
}

template <typename T>
size_t LinkedList<T>::remove(Where where) noexcept
{
	size_t opCount = 0;

	auto prev = head;
	auto cur = prev->next;

	while (cur != nullptr)
	{
		if (where(static_cast<LinkValueNode<T>*>(cur)->value))
		{
			removeData(prev, cur);
			opCount++;
			size--;
		}
		else
		{
			prev = cur;
		}
		cur = prev->next;
	}

	return opCount;
}

template <typename T>
void LinkedList<T>::removeAt(size_t index) noexcept
{
	if (index >= size)
		return;

	auto prev = head;
	auto cur = prev->next;
	while (index)
	{
		prev = cur;
		cur = prev->next;
		index--;
	}

	removeData(prev, cur);
	size--;
}

template <typename T>
size_t LinkedList<T>::update(Where where, std::function<void(T& oldData)> updateSet) noexcept
{
	size_t resCount = 0;

	auto cur = head->next;
	while (cur != nullptr)
	{
		if (where(static_cast<LinkValueNode<T>*>(cur)->value))
		{
			updateSet(static_cast<LinkValueNode<T>*>(cur)->value);
			resCount++;
		}
		cur = cur->next;
	}

	return resCount;

}

template <typename T>
void LinkedList<T>::updateAt(size_t index, T newData) noexcept
{
	if (index >= size)
		return;

	auto cur = head->next;
	while (index)
	{
		cur = cur->next;
		index--;
	}

	cur->value = newData;
}

//template <typename T>
//size_t LinkedList<T>::select(Where where, const T*& result) const
//{
//	size_t resCount = count(where);
//	T* resPtr = new T[resCount];
//	size_t curIndex = 0;
//	select(where, [&resPtr, &curIndex](const T& data)
//		{
//			resPtr[curIndex++] = data;
//		});
//	result = resPtr;
//	return resCount;
//}

template <typename T>
size_t LinkedList<T>::select(Where where, std::function<void(const T& data)> result) const noexcept
{
	size_t resCount = 0;

	auto cur = head->next;
	while (cur != nullptr)
	{
		if (where(static_cast<LinkValueNode<T>*>(cur)->value))
		{
			result(static_cast<LinkValueNode<T>*>(cur)->value);
			resCount++;
		}
		cur = cur->next;
	}

	return resCount;
}

template <typename T>
const T& LinkedList<T>::selectAt(size_t index) const noexcept(false)
{
	if (index >= size)
		throw std::out_of_range("链表索引超出范围");

	auto cur = head->next;
	while (index)
	{
		cur = cur->next;
		index--;
	}
	return static_cast<LinkValueNode<T>*>(cur)->value;
}

template <typename T>
size_t LinkedList<T>::count() const noexcept
{
	return size;
}

template<typename T>
size_t LinkedList<T>::count(Where where) const noexcept
{
	size_t resCount = 0;

	auto cur = head->next;
	while (cur != nullptr)
	{
		if (where(static_cast<LinkValueNode<T>*>(cur)->value))
		{
			resCount++;
		}
		cur = cur->next;
	}

	return resCount;
}

template<typename T>
inline void LinkedList<T>::clear() noexcept
{
	auto cur = head->next;
	head->next = nullptr;
	while (cur != nullptr)
	{
		auto next = cur->next;
		delete cur;
		cur = next;
	}
	size = 0;
}

template <typename T>
LinkedList<T>::~LinkedList() noexcept
{
	if (head != nullptr)
	{
		// 循环释放 LinkNode
		auto cur = head;
		auto next = cur->next;
		while (next != nullptr)
		{
			delete cur;
			cur = next;
			next = cur->next;
		}
		delete cur;
		cur = nullptr;

		head = nullptr;
	}
}


#pragma endregion
