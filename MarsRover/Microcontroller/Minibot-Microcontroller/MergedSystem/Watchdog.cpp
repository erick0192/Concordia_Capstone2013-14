#include <Wire.h>
#include "Arduino.h"
#include "TimerOne.h"
#include "Watchdog.h"
#include "Globals.h"


Watchdog watchdog;

boolean Watchdog::_freeToMove = true;
boolean Watchdog::_dataReceived = false;

void Watchdog::initialize(unsigned long microseconds=1000000)
{
  Timer1.initialize(microseconds);
  Timer1.attachInterrupt(watchdog.timeout);
}

boolean Watchdog::canMove(void)
{
  return _freeToMove;
}

void Watchdog::reportActivity(void)
{
  _dataReceived = true;
  Timer1.restart();
  _freeToMove = true;
}

void Watchdog::timeout(void)
{
  if (_dataReceived)
  {
    _dataReceived = false;
  }
  else
  {
    //StopMove();
    
    Wire.beginTransmission(LEFT_DEVICE_ADDRESS);
    Wire.write("F000F000F000");
    Wire.endTransmission();
    
    Wire.beginTransmission(RIGHT_DEVICE_ADDRESS);
    Wire.write("F000F000F000");
    Wire.endTransmission();
    
    _freeToMove = false;
  }
}
