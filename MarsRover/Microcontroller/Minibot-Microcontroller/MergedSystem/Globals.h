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
  const char I2C_LEFT = 'L';
  const char I2C_RIGHT = 'R';
}

#define LEFT_DEVICE_ADDRESS 2
#define RIGHT_DEVICE_ADDRESS 3

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
#define SERVO_PAN_STOP_COMMAND 1521

//Camera Servo Pan Tilt Pin
#define SERVO_CAM_1_ID 1
#define SERVO_CAM_1_PAN_PIN 25
#define SERVO_CAM_1_TILT_PIN 26

#define SERVO_CAM_2_ID 2
#define SERVO_CAM_2_PAN_PIN 27
#define SERVO_CAM_2_TILT_PIN 28

#define SERVO_CAM_3_ID 3
#define SERVO_CAM_3_PAN_PIN 29
#define SERVO_CAM_3_TILT_PIN 30

#define SERVO_CAM_4_ID 4
#define SERVO_CAM_4_PAN_PIN 31
#define SERVO_CAM_4_TILT_PIN 32

// Set your serial port baud rate used to send out data here:
#define OUTPUT_BAUD_RATE 115200
#endif
