/*
 * Example use of the uC watchdog
 *
 * By Jean-Robert Harvey
 */

#include "Watchdog.h"
#include <TimerOne.h>

int ledPin = 7;
  
void setup()
{
  Serial.begin(9600);
  pinMode(ledPin, OUTPUT);
  watchdog.initialize(3000000);
}

void loop()
{
  if (watchdog.canMove())
  {
    digitalWrite(ledPin, HIGH);
  }
  else
  {
    digitalWrite(ledPin, LOW);
  }
  
  if (Serial.available() > 0)
  {
    Serial.write("Received : ");
    
    while (Serial.available() > 0)
    {
      Serial.write(Serial.read());
    }
    Serial.write("\n");
    
    watchdog.reportActivity();
  }
  
  delay(100);
}
