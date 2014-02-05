#ifndef Camera_h
#define Camera_h
#include "Arduino.h"
#include "Servo.h"

class Camera
{
  public:
    Camera(int pinServoPan, int pinServoTilt, int camID);
    void Pan(int angle);
    void Tilt(int angle);
    int cameraID;
    static int numCam;
    
    private:
    int _pinServoPan;
    int _pinServoTilt;
    Servo _servoPan;
    Servo _servoTilt;
  };
#endif
