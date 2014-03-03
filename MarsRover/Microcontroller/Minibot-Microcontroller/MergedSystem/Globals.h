#ifndef Globals_h
#define Globals_h

namespace CommandMetadata{
  const char COMMAND_START = '<';
  const char COMMAND_END = '>';
  const char MOTOR_COMMAND = 'M';
  const char MOVE_FORWARD = 'F';
  const char MOVE_BACKWARD = 'B';
  const char SERVO_PAN = 'P';
  const char SERVO_TILT = 'T';
  const char SERIAL_KEEP_ALIVE = 'K';
  
}

#define TOP_RIGHT_WHEEL_ENABLE_PIN 4
#define TOP_RIGHT_WHEEL_FRONT_PIN 49
#define TOP_RIGHT_WHEEL_BACK_PIN 47

#define TOP_LEFT_WHEEL_ENABLE_PIN 5
#define TOP_LEFT_WHEEL_FRONT_PIN 48
#define TOP_LEFT_WHEEL_BACK_PIN 50

#define BOTTOM_RIGHT_WHEEL_ENABLE_PIN 6
#define BOTTOM_RIGHT_WHEEL_FRONT_PIN 35
#define BOTTOM_RIGHT_WHEEL_BACK_PIN 37

#define BOTTOM_LEFT_WHEEL_ENABLE_PIN 7
#define BOTTOM_LEFT_WHEEL_FRONT_PIN 41
#define BOTTOM_LEFT_WHEEL_BACK_PIN 43

// GPS uses Serial1 on Mega
// Pin 19 is RX and Pin 18 is TX

#define SERVO_STABILIZER_YAW_PIN 9
#define SERVO_STABILIZER_PITCH_PIN 10
#define SERVO_STABILIZER_ROLL_PIN 11

//Camera Servo Pan Tilt Pin
#define SERVO_CAM_1_ID 1
#define SERVO_CAM_1_PAN_PIN 8
#define SERVO_CAM_1_TILT_PIN 12

// Set your serial port baud rate used to send out data here:
#define OUTPUT_BAUD_RATE 115200
#endif
