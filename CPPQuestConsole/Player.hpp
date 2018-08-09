//
//  Player.hpp
//  CPPQuest
//
//  Created by Marcelo Chaves on 09/08/18.
//  Copyright © 2018 Dormir Não Dá XP. All rights reserved.
//

#pragma once
#include "Printable.hpp"
#include "GameObject.hpp"
#include "Item.hpp"

#include <iostream>
// #include "Windows.h" // How to include functions HANDLE, STD_OUTPUT_HANDLE and COORD

using namespace std;

class Player :

public Printable, public GameObject {
public:
    int points;
    
    Player (int x, int y,  char mc = '#', int fc = 3, int bc = 0 ):
    Printable (fc, bc, mc),
    GameObject (x,y) { this->points = 0; }
    
    ~Player(void);
    
    virtual void Print () {
        HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
        int k = getForeColor() + getBackColor()*16;
        COORD pos;
        pos.X = getX();
        pos.Y = getY();
        
        SetConsoleCursorPosition(hConsole, pos);
        SetConsoleTextAttribute(hConsole, k);
        cout << getChar();
    }
    
    void Move (char direction) {
        bool moved = true;
        int oldX = getX();
        int oldY = getY();
        
        switch(direction){
            case 'a': setX(getX() - 1);
                break;
            case 'd': setX(getX() + 1);
                break;
            case 'w': setY(getY() - 1);
                break;
            case 's': setY(getY() + 1);
                break;
            default: moved = false;
                break;
        }
        if (getX() > 70) {
            moved = false;
            setX(70);
        } else if (getX() < 1){
            moved = false;
            setX(1);
        }
        if (getY() > 20) {
            moved = false;
            setY(20);
        } else if (getY() < 1) {
            moved = false;
            setY(1);
        }
        if (moved) {
            HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
            int k = 0;
            COORD pos;
            pos.X = oldX;
            pos.Y = oldY;
            SetConsoleCursorPosition(hConsole, pos);
            SetConsoleTextAttribute(hConsole, k);
            cout <<' ';
        }
    }
    
    void EatItem (Item *i) {
        this->points += i->value;
    }
};
