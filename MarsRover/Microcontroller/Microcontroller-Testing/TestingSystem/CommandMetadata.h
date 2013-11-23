#ifndef CommmandMetadata_h
#define CommandMetadata_h

namespace CommandMetadata{
	 const int MAX_COMMAND_SIZE = 11; //Length of longest command
	 const char COMMAND_START = '<';
	 const char COMMAND_END = '>';
         const char MOVE_COMMAND = 'M';
	 const char MOVE_FORWARD = 'F';
	 const char MOVE_BACKWARD = 'B';
         const char SERVO_COMMAND = 'S';
}
#endif
