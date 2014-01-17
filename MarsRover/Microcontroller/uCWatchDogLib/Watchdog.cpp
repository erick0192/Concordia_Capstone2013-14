#include "Arduino.h"
#include "TimerOne.h"
#include "Watchdog.h"

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
    _freeToMove = false;
  }
}

