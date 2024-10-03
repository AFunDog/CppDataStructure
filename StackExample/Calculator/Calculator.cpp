#include "Calculator.h"
#include "../Stack/Stack.h"
#include <stdexcept>
#include <vector>
#include <map>
#include <sstream>
#include <iostream>
#include <typeinfo>

using namespace std;

enum OperatorType {
	Null,
	Add,
	Sub,
	Mul,
	Div
};
class MathUnit {

	bool isNumber;
	double number = 0;
	OperatorType op = Null;
public:


	MathUnit(double number) {
		isNumber = true;
		this->number = number;
	}

	MathUnit(char op) {
		isNumber = false;
		switch (op)
		{
		case '+':
			this->op = Add;
			break;
		case '-':
			this->op = Sub;
			break;
		case '*':
			this->op = Mul;
			break;
		case '/':
			this->op = Div;
			break;
		default:
			break;
		}
	}

	bool IsNumber() const {
		return isNumber;
	}
	const double& GetNumber() const {
		return number;
	}
	bool IsOperator() const {
		return !isNumber;
	}
	const OperatorType& GetOperator() const {
		return op;
	}
};


struct MathExp {
	vector<MathUnit> units;
};


static MathExp InfixToSuffix(const string& infix) noexcept(false)
{
	map<char, int> priority = {
		{'(',-1},
		{'+',1},
		{'-',1},
		{'*',2},
		{'/',2},
	};
	istringstream ss(infix);
	MathExp exp;
	size_t curPos = 0;
	size_t length = infix.size();
	Stack<char> ops;
	while (curPos < length) {
		auto& c = infix[curPos];
		if ('0' <= c && c <= '9' || ('-' == c && '0' <= infix[curPos + 1] && infix[curPos + 1] <= '9')) {
			double number = 0;
			ss >> number;
			exp.units.push_back(MathUnit(number));
		}
		else {
			char op;
			ss >> op;
			if (op == '(') {
				ops.Push(op);
			}
			else if (op == ')') {
				bool isMatched = false;
				while (ops.IsEmpty() == false) {
					auto& sop = ops.Pop();
					if (sop == '(') {
						isMatched = true;
						break;
					}
					exp.units.push_back(MathUnit(sop));
				}
				if (isMatched == false) {
					throw invalid_argument("括号不匹配");
				}
			}
			else {
				while (ops.IsEmpty() == false) {
					if (priority[ops.Top()] >= priority[op]) {
						exp.units.push_back(MathUnit(ops.Pop()));
					}
					else {
						break;
					}
				}
				ops.Push(op);
			}
		}
		curPos = ss.tellg();
		if (curPos == EOF)break;
	}
	while (ops.IsEmpty() == false)
	{
		exp.units.push_back(MathUnit(ops.Pop()));
	}
	return exp;
}


double Calculator::Calculate(const string& expression) noexcept(false)
{
	auto units = InfixToSuffix(expression).units;
	Stack<double> stack;
	for (auto& unit : units) {
		if (unit.IsNumber()) {
			stack.Push(unit.GetNumber());
		}
		else {
			if (stack.Size() < 2 && unit.GetOperator() != Sub && unit.GetOperator() != Add) {
				throw invalid_argument("表达式无效 操作符两边应都是操作数");
			}
			double& number1 = stack.Pop();
			double& number2 = stack.Pop();
			switch (unit.GetOperator()) {
			case Add:
				stack.Push(number2 + number1);
				break;
			case Sub:
				stack.Push(number2 - number1);
				break;
			case Mul:
				stack.Push(number2 * number1);
				break;
			case Div:
				if (number1 == 0) {
					throw invalid_argument("除数不能为0");
				}
				stack.Push(number2 / number1);
				break;
			default:
				throw invalid_argument("表达式无效 无效操作符");
			}
		}
	}
	return stack.Pop();
}

