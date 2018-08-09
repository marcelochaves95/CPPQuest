//
//  Item.hpp
//  CPPQuest
//
//  Created by Marcelo Chaves on 09/08/18.
//  Copyright © 2018 Dormir Não Dá XP. All rights reserved.
//

#pragma once
#include "Printable.hpp"
#include "GameObject.hpp"
#include <iostream>
// #include "Windows.h" /* How to include functions HANDLE, STD_OUTPUT_HANDLE and COORD */

using namespace std;

class Item :

public Printable, public GameObject {
public:
    int value;
    
    Item (int x, int y, int v, char mc = '#', int fc = 14, int bc = 0):
    Printable (fc, bc, mc),
    GameObject (x, y) {
        this->value = v;
    }
    
    ~Item(void);
    
    virtual void Print () {
        HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);;
        int k = getForeColor() + getBackColor()*16;
        COORD pos;
        pos.X = getX();
        pos.Y = getY();
        
        SetConsoleCursorPosition(hConsole, pos);
        SetConsoleTextAttribute(hConsole, k);
        cout << getChar();
    }
};
