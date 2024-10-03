#include "Calculator.h"
#include <iostream>

using namespace std;



int main(int argc, char* argv[]) {
	try
	{
		if (argc <= 1) {
			throw invalid_argument("空表达式");
		}
		cout << Calculator::Calculate(argv[1]) << endl;
	}
	catch (const exception& e)
	{
		cerr << "Error " << e.what() << endl;
	}


}