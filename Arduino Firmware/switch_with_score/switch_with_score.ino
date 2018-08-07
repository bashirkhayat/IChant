#include <CapacitiveSensor.h>

#define ON_CLICK 1
#define TEMPO 2

int mode = -1;
long inputs[8] = {0};
long prev_inputs[8] = {0};

int Left_Thumb = 13;      
int Left_Index  = 12;
int Left_Middle = 11;
int Left_Ring = 9;
int Right_Index = 7;
int Right_Middle = 6;
int Right_Ring = 5; 
int Right_Pinky = 4;

CapacitiveSensor Left_Thumb_cap =   CapacitiveSensor(28,22);
CapacitiveSensor Left_Index_cap =   CapacitiveSensor(29,23);
CapacitiveSensor Left_Middle_cap =  CapacitiveSensor(36,30);
CapacitiveSensor Left_Ring_cap =    CapacitiveSensor(37,31);
CapacitiveSensor Right_Index_cap =  CapacitiveSensor(44,38);
CapacitiveSensor Right_Middle_cap = CapacitiveSensor(45,39);
CapacitiveSensor Right_Ring_cap =   CapacitiveSensor(52,46);
CapacitiveSensor Right_Pinky_cap =  CapacitiveSensor(53,47);


int DetectCombination(){
  if (inputs[0] == 1 && inputs[1] == 1 && inputs[2] == 1 && inputs[3] == 1 && inputs[4] == 1 && inputs[5] == 1 && inputs[6] == 1 && inputs[7] == 1)//11111111
        return 55;
   else if (inputs[0] == 1 && inputs[1] == 1 && inputs[2] == 1 && inputs[3] == 1 && inputs[4] == 1 && inputs[5] == 1 && inputs[6] == 1 
    && inputs[7] == 0)//11111110
        return 57;
   else if (inputs[0] == 1 && inputs[1] == 1 && inputs[2] == 1 && inputs[3] == 1 && inputs[4] == 1 && inputs[5]  == 1
    && inputs[6] == 0 && inputs[7] == 0)//11111100
        return 59;     
   else if (inputs[0] == 1 && inputs[1] == 1 && inputs[2] == 1 && inputs[3] == 1 && inputs[4] == 1 && inputs[7]  == 1
    && inputs[5] == 0 && inputs[6] == 0)//11111001
        return 61;
   else if (inputs[0] == 1 && inputs[1] == 1 && inputs[2] == 1 && inputs[3] == 1 && inputs[7] == 1
    && inputs[4] == 0 && inputs[5] == 0 && inputs[6] == 0)//11110001
        return 62;
   else if (inputs[0] == 1 && inputs[1] == 1 && inputs[2] == 1 && inputs[4]  == 1 && inputs[5] == 1 && inputs[6] == 1
    && inputs[3] == 0 && inputs[7] == 0)//11101110
        return 64;
   else if (inputs[0] == 1 && inputs[1] == 1 && inputs[4]  == 1 && inputs[5] == 1 && inputs[6] == 1
    && inputs[2] == 0 && inputs[3] == 0 && inputs[7] == 0)//11001110
        return 66;
   else if (inputs[0] == 1 && inputs[4]  == 1 && inputs[5] == 1 && inputs[6] == 1
    && inputs[1] == 0 && inputs[2] == 0 && inputs[3] == 0 && inputs[7] == 0)//10001110
        return 67;
   else if (inputs[3] == 1 && inputs[4]  == 1 && inputs[5] == 1 && inputs[6] == 1
    && inputs[0] == 0 && inputs[1] == 0 && inputs[2] == 0 && inputs[7] == 0)//00011110
        return 69;
   else
      return -1;
}

int CapSensorHandler(){
  for ( int i = 0; i < 8; i ++)
    prev_inputs[i] = inputs[i];
  
  inputs[0] = (Left_Thumb_cap.capacitiveSensor(30) > 100) ? 1 : 0;
  inputs[1] = (Left_Index_cap.capacitiveSensor(30) > 100) ? 1 : 0;
  inputs[2] = (Left_Middle_cap.capacitiveSensor(30) > 100) ? 1 : 0;
  inputs[3] = (Left_Ring_cap.capacitiveSensor(30) > 100) ? 1 : 0;
  inputs[4] = (Right_Index_cap.capacitiveSensor(30) > 100) ? 1 : 0;
  inputs[5] = (Right_Middle_cap.capacitiveSensor(30) > 100) ? 1 : 0;
  inputs[6] = (Right_Ring_cap.capacitiveSensor(30) > 100) ? 1 : 0;
  inputs[7] = (Right_Pinky_cap.capacitiveSensor(30) > 100) ? 1 : 0;
 
  return DetectCombination(); 
}


bool changedCombination() {
  for ( int i = 0; i < 8; i ++)
    if (prev_inputs[i] != inputs[i])
      return true;
  return false;
}
  
void LedHandler(int note){
  switch (note) {
    case 0: { 
      digitalWrite(Left_Thumb,LOW);
      digitalWrite(Left_Index,LOW);
      digitalWrite(Left_Middle,LOW);
      digitalWrite(Left_Ring,LOW);
      digitalWrite(Right_Index,LOW);
      digitalWrite(Right_Middle,LOW);
      digitalWrite(Right_Ring,LOW);      
      digitalWrite(Right_Pinky,LOW);  
      break;
    }
    case 55: { //Low-G
      digitalWrite(Left_Thumb,HIGH);
      digitalWrite(Left_Index,HIGH);
      digitalWrite(Left_Middle,HIGH);
      digitalWrite(Left_Ring,HIGH);
      digitalWrite(Right_Index,HIGH);
      digitalWrite(Right_Middle,HIGH);
      digitalWrite(Right_Ring,HIGH);      
      digitalWrite(Right_Pinky,HIGH);      
      break;
    }
    case 57: { //Low-A
      digitalWrite(Left_Thumb,HIGH);
      digitalWrite(Left_Index,HIGH);
      digitalWrite(Left_Middle,HIGH);
      digitalWrite(Left_Ring,HIGH);
      digitalWrite(Right_Index,HIGH);
      digitalWrite(Right_Middle,HIGH);
      digitalWrite(Right_Ring,HIGH);      
      digitalWrite(Right_Pinky,LOW); 
      break;
    }
    case 59: { //B
      digitalWrite(Left_Thumb,HIGH);
      digitalWrite(Left_Index,HIGH);
      digitalWrite(Left_Middle,HIGH);
      digitalWrite(Left_Ring,HIGH);
      digitalWrite(Right_Index,HIGH);
      digitalWrite(Right_Middle,HIGH);
      digitalWrite(Right_Ring,LOW);      
      digitalWrite(Right_Pinky,LOW);  
      break;
    }
    case 61: { //C
      digitalWrite(Left_Thumb,HIGH);
      digitalWrite(Left_Index,HIGH);
      digitalWrite(Left_Middle,HIGH);
      digitalWrite(Left_Ring,HIGH);
      digitalWrite(Right_Index,HIGH);
      digitalWrite(Right_Middle,LOW);
      digitalWrite(Right_Ring,LOW);      
      digitalWrite(Right_Pinky,HIGH); 
      break;
    }
    case 62: { //D
      digitalWrite(Left_Thumb,HIGH);
      digitalWrite(Left_Index,HIGH);
      digitalWrite(Left_Middle,HIGH);
      digitalWrite(Left_Ring,HIGH);
      digitalWrite(Right_Index,LOW);
      digitalWrite(Right_Middle,LOW);
      digitalWrite(Right_Ring,LOW);      
      digitalWrite(Right_Pinky,HIGH); 
      break;
    }
    case 64: { //E
      digitalWrite(Left_Thumb,HIGH);
      digitalWrite(Left_Index,HIGH);
      digitalWrite(Left_Middle,HIGH);
      digitalWrite(Left_Ring,LOW);
      digitalWrite(Right_Index,HIGH);
      digitalWrite(Right_Middle,HIGH);
      digitalWrite(Right_Ring,HIGH);      
      digitalWrite(Right_Pinky,LOW); 
      break;
    }
    case 66: { //F
      digitalWrite(Left_Thumb,HIGH);
      digitalWrite(Left_Index,HIGH);
      digitalWrite(Left_Middle,LOW);
      digitalWrite(Left_Ring,LOW);
      digitalWrite(Right_Index,HIGH);
      digitalWrite(Right_Middle,HIGH);
      digitalWrite(Right_Ring,HIGH);      
      digitalWrite(Right_Pinky,LOW);      
      break;
    }
    case 67: { //High-G
      digitalWrite(Left_Thumb,HIGH);
      digitalWrite(Left_Index,LOW);
      digitalWrite(Left_Middle,LOW);
      digitalWrite(Left_Ring,LOW);
      digitalWrite(Right_Index,HIGH);
      digitalWrite(Right_Middle,HIGH);
      digitalWrite(Right_Ring,HIGH);      
      digitalWrite(Right_Pinky,LOW); 
      break;
    }
    case 69: { //High-A
      digitalWrite(Left_Thumb,LOW);
      digitalWrite(Left_Index,LOW);
      digitalWrite(Left_Middle,LOW);
      digitalWrite(Left_Ring,HIGH);
      digitalWrite(Right_Index,HIGH);
      digitalWrite(Right_Middle,HIGH);
      digitalWrite(Right_Ring,HIGH);      
      digitalWrite(Right_Pinky,LOW); 
      break;
    }
  }
}

void SetOutputs(){
  pinMode(Left_Thumb,OUTPUT);
  pinMode(Left_Index,OUTPUT);
  pinMode(Left_Middle,OUTPUT);
  pinMode(Left_Ring,OUTPUT);
  pinMode(Right_Index,OUTPUT);
  pinMode(Right_Middle,OUTPUT);
  pinMode(Right_Ring,OUTPUT);      
  pinMode(Right_Pinky,OUTPUT); 
}

void CalibrateSensors(){
  Left_Thumb_cap.set_CS_AutocaL_Millis(0xFFFFFFFF);
  Left_Index_cap.set_CS_AutocaL_Millis(0xFFFFFFFF);
  Left_Middle_cap.set_CS_AutocaL_Millis(0xFFFFFFFF);
  Left_Ring_cap.set_CS_AutocaL_Millis(0xFFFFFFFF);
  Right_Index_cap.set_CS_AutocaL_Millis(0xFFFFFFFF);
  Right_Middle_cap.set_CS_AutocaL_Millis(0xFFFFFFFF);
  Right_Ring_cap.set_CS_AutocaL_Millis(0xFFFFFFFF);
  Right_Pinky_cap.set_CS_AutocaL_Millis(0xFFFFFFFF);
}

void setup() {
  SetOutputs();
  CalibrateSensors();
  Serial.begin(9600);
}

void OnClick() {
  int note, score, counter = 0;
  while (Serial.available() <= 0) {}    
  
  while (true) {
    while (Serial.available() <= 0) {}
    
    note = Serial.read();
    if (note == 0) break;
    while(true) {  
      LedHandler(note);
      if (note != CapSensorHandler()) {    
        if (changedCombination()) {
          counter = 0;
          LedHandler(note);
          delay(100);
          LedHandler(0);
          delay(100);
        }
      } else {
          counter++;;
          break;
      }
    }
    score = counter / 5 + 1;
    Serial.print(score);   //ask for the next note from app      
  }
  LedHandler(0);
}



void tempo(){
    int note;
    while (Serial.available() <= 0) {}    
    while (true) {
        while (Serial.available() <= 0) {}
        note = Serial.read();
        if (note == 0) break;
        LedHandler(note);  
    }
    LedHandler(0);
}


int input;
void loop() {
  while (Serial.available() <= 0) {}
  input = Serial.read();
//  if (mode == -1)
    mode = input;
  if (mode == ON_CLICK)
    OnClick();
  else if (mode == TEMPO)
    tempo();
}
