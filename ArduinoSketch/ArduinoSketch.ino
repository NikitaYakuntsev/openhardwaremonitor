#include <ArduinoJson.h>

enum HwType { CPU = 2, RAM = 3, GPU = 4 };
HwType currentHwType = CPU;
int currentShowState = 1; //increment this later on btn click.

int ind100Pin = 5;
int ind300Pin = 6;
const int PERCENT_LIMIT = 100;
const int TEMP_LIMIT = 100;
const int PIN_LIMIT = 255;

void setup() {
  Serial.begin(9600);
  while (!Serial) {
    // wait serial port initialization
  }  
  pinMode(ind100Pin, OUTPUT);
  pinMode(ind300Pin, OUTPUT);
  
  Init();
}

void loop() {
  // not used in this example
}

void serialEvent()
{
  char* json;
  String str = Serial.readStringUntil('\n');
  json = (char*) str.c_str();
  DynamicJsonBuffer jsonBuffer;
  Serial.println(json);
  JsonArray& root = jsonBuffer.parse(json);

  // Test if parsing succeeds.
  if (!root.success()) {
    return;
  }
  
  for (int i = 0; i < root.size(); i++) {
    JsonObject& curr = root[i];
    int hwType = curr["HwType"];
    int showState = curr["GroupId"];
    double value;
    int type;
    int indicator;
    if (showState == currentShowState) {
      value = curr["Value"];
      type = curr["Type"];
      indicator = curr["PrefIndicator"];
      
      bool ind300 = indicator == 300;
      int writePin = ind300 ? ind300Pin : ind100Pin;
      int writeValue = map(value, 0, ind300 ? TEMP_LIMIT : PERCENT_LIMIT, 0, PIN_LIMIT);
      analogWrite(writePin, writeValue);
    }       
  }
   

}

void Init() {
  digitalWrite(ind100Pin, HIGH);
  digitalWrite(ind300Pin, HIGH);  
  delay(500);
  digitalWrite(ind100Pin, LOW);
  digitalWrite(ind300Pin, LOW);
  delay(500);
  digitalWrite(ind100Pin, HIGH);
  digitalWrite(ind300Pin, HIGH);  
  delay(500);
  digitalWrite(ind100Pin, LOW);
  digitalWrite(ind300Pin, LOW);
  delay(500);
}


