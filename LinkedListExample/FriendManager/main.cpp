#include "../LinkedList/LinkedList.h"
#include "Friend.h"
#include "FriendManager.h"
#include <string>
#include <sstream>
#include <iostream>
#include <functional>
#include <map>

using namespace std;


static bool isExited = false;

/*
	存放指令对应的操作	调用 FriendManager 进行操作

	selectAll		获取所有好友信息
	select			根据条件获取好友信息
	insert			在指定位置添加好友信息
	remove			删除指定编号的好友信息
	update			更新指定编号的好友信息
	clear			清除所有好友信息
	exit			退出程序
	load			读取本地好友信息数据
	save			保存好友信息到本地
*/
map<wstring, function<void(wistream& ss)>> actionTable = {
	{L"selectAll",[&](wistream& ss) {
		FriendManager::SelectAll();
	}},
	{L"select",[&](wistream& ss) {
		int conditionNum;
		ss >> conditionNum;
		//vector<std::function<bool(const Friend& data)>> conditions;
		LinkedList<std::function<bool(const Friend& data)>> conditions;
		for (int i = 0; i < conditionNum; i++) {
			wstring key;
			ss >> key;
			if (key == L"name") {
				wstring value;
				while (ss.peek() == L' ') {
					ss.ignore();
				}
				getline(ss, value);
				conditions.insertLast([=](const Friend& data) { return data.name.compare(value) == 0; });
			}
			else if (key == L"birthYear") {
				int year;
				ss >> year;
				conditions.insertLast([=](const Friend& data) { return data.birthYear == year; });
			}
			else if (key == L"birthMonth") {
				int month;
				ss >> month;
				conditions.insertLast([=](const Friend& data) { return data.birthMonth == month; });
			}
			else if (key == L"picPath") {
				wstring value;
				while (ss.peek() == L' ') {
					ss.ignore();
				}
				getline(ss, value);
				conditions.insertLast([=](const Friend& data) { return data.picPath.compare(value) == 0; });
			}
			else if (key == L"hobby") {
				wstring value;
				while (ss.peek() == L' ') {
					ss.ignore();
				}
				getline(ss, value);
				conditions.insertLast([=](const Friend& data) { return data.hobby.compare(value) == 0; });
			}
			else {
				wcerr << "invalid key: " << key << endl;
			}
		}
		FriendManager::Select([&](const Friend& data) {
			bool res = true;
			conditions.select([&](auto& conData) {return res; }, [&](auto& conData) {
				if (conData(data) == false) {
					res = false;
				}
			});
			return res;
			}
		);
	}},
	{L"insert",[&](wistream& ss) {
		int index;
		Friend f;
		ss >> index >> f;
		FriendManager::Insert(index, f);
		wcerr << "insert " << f << endl;
	}},
	{L"remove",[&](wistream& ss) {
		int id;
		ss >> id;
		FriendManager::Remove(id);
	}},
	{L"update",[&](wistream& ss) {
		int id;
		Friend newData;
		ss >> id >> newData;
		FriendManager::Update(id, newData);
	}},
	{L"clear",[&](wistream& ss) {
		FriendManager::Clear();
	}},
	{L"exit",[&](wistream& ss) {
		isExited = true;
	}},
	{L"load",[&](wistream& ss) {
		FriendManager::Load();
	}},
	{L"save",[&](wistream& ss) {
		FriendManager::Save();
	}}
};




int main(int argc, char* argv[]) {
	setlocale(LC_ALL, "chs");
	wcin.imbue(locale("chs"));
	wcout.imbue(locale("chs"));


	/*
		不断获取命令直到程序退出
	*/
	while (isExited == false)
	{
		try
		{
			wstring action;
			wcin >> action;
			if (action.empty()) {
				isExited = true;
				throw invalid_argument("空命令 这可能意味着宿主线程已退出");
			}
			actionTable[action](wcin);
		}
		catch (const std::exception& e)
		{
			wcerr << e.what() << endl;
			wcin.clear();
		}
	}

}