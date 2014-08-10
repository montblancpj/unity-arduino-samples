#define SERVO_PIN 9
#define LED_PIN 13
#define SOLENOID_PIN 2
#define BUFFER_SIZE 32

#include <Servo.h>

Servo servo;
char buffer[BUFFER_SIZE];
void setup() 
{
  Serial.begin(9600);

  pinMode(LED_PIN, OUTPUT);
  pinMode(SOLENOID_PIN, OUTPUT);

  servo.attach(SERVO_PIN);
  servo.write(0);
} 

bool ReadDataAsInt(char data[], int &intData) 
{   
  bool ret = false;
  int i = 0;
  buffer[0] = '\0';
  while(Serial.available() ) {
    buffer[i] = Serial.read();
    if('\0' == buffer[i]) {
      ret = true;
      intData = atoi(buffer);
      break;
    }
    i++;
    if(i >= BUFFER_SIZE) break;
    delay(5); // for serial read delay
  };

  return ret;
}

void loop() 
{
  if(Serial.available() >= 2) {
    char textHead = Serial.read();
    if('s' == textHead) {
      int val = 0;
      bool ret = ReadDataAsInt(buffer, val);
      if(ret) ServoDrive(val);
    } else if('l' == textHead) {
      int val = -1;
      bool ret = ReadDataAsInt(buffer, val);
      
      if(ret) {
        if(val > 0 && val < 255) {
          digitalWrite(LED_PIN, HIGH);   
        } else if(val == 0) {
          digitalWrite(LED_PIN, LOW);   
        }
      }
    } else if('p' == textHead) {
      int val = 0;
      bool ret = ReadDataAsInt(buffer, val);
      if(ret) {
        if(val == 1) {
          digitalWrite(SOLENOID_PIN, HIGH);
        } else {
          digitalWrite(SOLENOID_PIN, LOW);   
        }
      }
    }
  }
} 

void ServoDrive(int val)
{
  if(val >= 0 && val < 255) {
    servo.write(val);
    delay(15);
  }
}
