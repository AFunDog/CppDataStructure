#pragma once

#include <string>

class Calculator
{
public:
	static double Calculate(const std::string& expression) noexcept(false);
};

