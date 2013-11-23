#include "Wheel.h"

Wheel::Wheel(int enablePin, int frontPin, int backPin):
_enablePin(enablePin), _frontPin(frontPin), _backPin(backPin) { }

Wheel::Wheel(): _enablePin(0), _frontPin(0), _backPin(0) { }


int Wheel::getEnablePin(){
	return _enablePin;
}

int Wheel::getFrontPin(){
	return _frontPin;
}

int Wheel::getBackPin(){
	return _backPin;
}

void Wheel::setEnablePin(int enablePin){
	_enablePin = enablePin;
}

void Wheel::setFrontPin(int frontPin){
	_frontPin = frontPin;
}

void Wheel::setBackPin(int backPin){
	_backPin = backPin;
}

void Wheel::setAll(int data[]){
        _enablePin = data[1];
        _frontPin = data[2];
        _backPin = data[3];
}
