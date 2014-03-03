/*
 * This watchdog library provides an easy way to set up
 * a watchdog mechanism on an Arduino board.  It uses the
 * TimerOne library as an interrupt timer.  The time is
 * reset every time the supervised condition happens.  If 
 * the timer period is allowed to elapse, the interrupt
 * handler is executed and the control function is set to
 * return false.
 *
 * TimerOne can be used both on the Arduino Uno and on the
 * Arduino Mega.  On the Uno, pins 9 and 10 arerequired for
 * for the timer.  On the Mega, that's pins 11, 12 and 13.
 */

#ifndef Watchdog_h
#define Watchdog_h

#include "Arduino.h"
#include "TimerOne.h"

class Watchdog
{
  public:
    void initialize(unsigned long);
    boolean canMove(void);
    void reportActivity(void);
    static void timeout(void);
    
  protected:
    static boolean _freeToMove;

    // Had to add this flag to avoid an interrupt being thrown
    // after restarting the interrupt timer.
    static boolean _dataReceived;
};

extern Watchdog watchdog;

#endif
