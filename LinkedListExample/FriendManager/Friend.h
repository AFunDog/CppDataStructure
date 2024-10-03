#pragma once

#include <string>
#include <sstream>

/*
	好友类 存放好友信息
*/
struct Friend
{
	Friend();
	Friend(const wchar_t* name, int birthYear, int birthMonth, const wchar_t* picPath, const wchar_t* hobby);

	~Friend();
	/*
		编号
	*/
	int id;
	/*
		姓名
	*/
	std::wstring name;
	/*
		出生年份
	*/
	int birthYear;
	/*
		出生月份
	*/
	int birthMonth;
	/*
		头像图片路径
	*/
	std::wstring picPath;
	/*
		爱好
	*/
	std::wstring hobby;

	std::wstring toString() const;

	// 输入输出流

	friend std::wistream& operator>>(std::wistream& ss,Friend& data);
	friend std::wostream& operator<<(std::wostream& ss, const Friend& data);
};

