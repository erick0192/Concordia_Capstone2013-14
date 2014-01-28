#include "Arduino.h"
#include "Servo.h"

#ifndef Camera_h
#define Camera_h
class Camera
{
  public:
    Camera(int pinServoPan, int pinServoTilt);
    void Pan(int angle);
    void Tilt(int angle);
  private:
    int _pinServoPan;
    int _pinServoTilt;
    Servo _servoPan;
    Servo _servoTilt;
};
#endif
