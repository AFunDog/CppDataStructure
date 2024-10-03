#pragma once
#include <initializer_list>
#include <math.h>
#include <stdexcept>
#include <exception>


/*

	模板定义部分

*/

template<typename T>
class Stack
{
	const size_t InitCapacity = 10;
public:
	Stack(size_t initCapacity = 10) noexcept;
	Stack(std::initializer_list<T> list, size_t initCapacity = InitCapacity) noexcept;

	T& Top() noexcept(false);
	T& Pop() noexcept(false);
	void Push(T data) noexcept;
	size_t Size() noexcept;
	bool IsEmpty() noexcept;
	void Clear() noexcept;

	~Stack() noexcept;
private:
	size_t capacity;
	size_t size;
	T* datas;
};

/*

	模板的实现部分
	这部分无法单独写到一个文件里，因为这样会产生编译错误

*/

#pragma region TemplateImpl

#pragma region Helpers


inline static size_t AllocateCapacity(size_t curCapacity, size_t curSize)
{
	if (curCapacity > curSize)
	{
		return curCapacity;
	}
	else
	{
		return floor(curSize * 1.5);
	}
}

template<typename T>
inline static void CopyTo(T* src, T* dest, size_t size)
{
	for (size_t i = 0; i < size; i++)
	{
		dest[i] = src[i];
	}
}

#pragma endregion


template<typename T>
inline Stack<T>::Stack(size_t initCapacity) noexcept
	: Stack({}, initCapacity)
{
}

template<typename T>
inline Stack<T>::Stack(std::initializer_list<T> list, size_t initCapacity) noexcept
{
	capacity = AllocateCapacity(initCapacity, list.size());
	datas = new T[capacity];
	int i = 0;
	for (T data : list)
	{
		datas[i] = data;
		i++;
	}

}

template<typename T>
inline T& Stack<T>::Top() noexcept(false)
{
	if (size == 0)
	{
		throw std::out_of_range("栈为空");
	}
	return datas[size - 1];
}

template<typename T>
inline T& Stack<T>::Pop() noexcept(false)
{
	if (size == 0)
	{
		throw std::out_of_range("栈为空");
	}
	size--;
	return datas[size];
}

template<typename T>
inline void Stack<T>::Push(T data) noexcept
{
	if (size == capacity)
	{
		capacity = AllocateCapacity(capacity, size + 1);
		T* newDatas = new T[capacity];
		CopyTo(datas, newDatas, size);
		delete[] datas;
		datas = newDatas;
	}
	datas[size++] = data;
}

template<typename T>
inline size_t Stack<T>::Size() noexcept
{
	return size;
}

template<typename T>
inline bool Stack<T>::IsEmpty() noexcept
{
	return size == 0;
}

template<typename T>
inline void Stack<T>::Clear() noexcept
{
	delete[] datas;
	datas = new T[InitCapacity];
	size = 0;
}

template<typename T>
inline Stack<T>::~Stack() noexcept
{
	if (datas != nullptr)
	{
		delete[] datas;
		datas = nullptr;
		size = 0;
	}
}
#pragma endregion

