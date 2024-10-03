#include "Friend.h"
#include <sstream>
#include <string>

using namespace std;
// 内部标识好友唯一编号
static int nextId = 0;


Friend::Friend() : Friend(L"", 0, 0, L"", L"")
{
}



Friend::Friend(const wchar_t* name, int birthYear, int birthMonth, const wchar_t* picPath, const wchar_t* hobby)
	: name(name), birthYear(birthYear), birthMonth(birthMonth), picPath(picPath), hobby(hobby)
{
	id = nextId++;
}

Friend::~Friend()
{

}

//void Friend::setId(int id)
//{
//	this->id = id;
//	nextId = std::max(nextId, id + 1);
//}

std::wstring Friend::toString() const
{
	auto ss = std::wstringstream();
	ss << id << L'\n' << name << L'\n' << birthYear << " " << birthMonth << L'\n' << picPath << L'\n' << hobby;
	return ss.str();
}

std::wistream& operator>>(std::wistream& ss, Friend& data)
{
	if (ss.peek() == L'\n') {
		ss.ignore();
	}
	getline(ss, data.name);
	ss >> data.birthYear >> data.birthMonth;
	if (ss.peek() == L'\n') {
		ss.ignore();
	}
	getline(ss, data.picPath);
	getline(ss, data.hobby);
	return ss;
}

std::wostream& operator<<(std::wostream& ss, const Friend& data)
{
	ss << data.name << L'\n' << data.birthYear << L' ' << data.birthMonth << L'\n' << data.picPath << L'\n' << data.hobby;
	return ss;
}
