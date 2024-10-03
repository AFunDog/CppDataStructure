#include "FriendManager.h"
#include "../LinkedList/LinkedList.h"
#include <iostream>
#include <fstream>

static LinkedList<Friend> friends;

void FriendManager::SelectAll()
{
	wcout << friends.count() << endl;
	friends.select([](auto& data) {return true; }, [&](const Friend& data) { wcout << data.toString() << endl; });
}

void FriendManager::Select(function<bool(const Friend& data)> where)
{
	wstringstream temps;
	auto opCount = friends.select(where, [&](const Friend& data) { temps << data.toString() << endl; });
	wcout << opCount << endl;
	wcout << temps.str();
}

void FriendManager::Insert(int index, const Friend& data)
{
	friends.insert(data, index);
}

void FriendManager::Remove(int id)
{
	friends.remove([&](const Friend& data) {return data.id == id; });
}

void FriendManager::Update(int id, const Friend& data)
{
	friends.update([&](const Friend& data) { return data.id == id; }, [&](Friend& oldData) {
		oldData.name = data.name;
		oldData.birthYear = data.birthYear;
		oldData.birthMonth = data.birthMonth;
		oldData.picPath = data.picPath;
		oldData.hobby = data.hobby;
		}
	);
}

void FriendManager::Load()
{
	wifstream fin;
	fin.imbue(locale("chs"));
	try
	{
		fin.open(".friends");
		if (fin.is_open() == false) {
			throw exception("文件不存在");
		}
		int num;
		fin >> num;

		for (int i = 0; i < num; i++) {
			Friend f;
			fin >> f;
			friends.insertLast(f);
		}
	}
	catch (const std::exception& e)
	{
		wcerr << L"read " << e.what() << endl;
	}
	fin.close();
}

void FriendManager::Save()
{
	wofstream fout;
	fout.imbue(locale("chs"));
	try
	{
		fout.open(".friends");
		if (fout.is_open() == false) {
			throw exception("文件不存在");
		}
		fout << friends.count() << endl;
		friends.select([](auto& data) {return true; }, [&](const Friend& data) { fout << data << endl; wcerr << data << endl; });
		fout << endl;
	}
	catch (const std::exception& e)
	{
		wcerr << L"save " << e.what() << endl;
	}
	fout.close();
}

void FriendManager::Clear()
{
	friends.clear();
}
