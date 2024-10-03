#pragma once
#include "Friend.h"
#include <string>
#include <functional>
using namespace std;



/*
	好友信息管理的主控类
*/
class FriendManager
{
	FriendManager() {}
public:

	/*
		获取所有好友信息
	*/
	static void SelectAll();
	/*
		根据条件获取好友信息
	*/
	static void Select(function<bool(const Friend& data)> where);
	/*
		在指定位置添加好友信息
	*/
	static void Insert(int index,const Friend& data);
	/*
		删除指定编号的好友信息
	*/
	static void Remove(int id);
	/*
		更新指定编号的好友信息
	*/
	static void Update(int id, const Friend& data);
	/*
		读取本地好友信息数据
	*/
	static void Load();
	/*
		保存好友信息到本地
	*/
	static void Save();
	/*
		清除所有好友信息
	*/
	static void Clear();
};

