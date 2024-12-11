int button_Switch1 = 0; 
int lastButtonState1 = HIGH;  
unsigned long lastDebounceTime1 = 0;  
unsigned long debounceDelay = 50;    

int button_Switch2 = 0;
int lastButtonState2 = HIGH;
unsigned long lastDebounceTime2 = 0;

int button_Switch3 = 0;
int lastButtonState3 = HIGH;
unsigned long lastDebounceTime3 = 0;

void setup() {
  Serial.begin(9600);        
  pinMode(4, INPUT);         
  pinMode(8, INPUT_PULLUP);  
  pinMode(9, INPUT_PULLUP);  
  pinMode(10, INPUT_PULLUP); 
}

void loop() {
  int button1Value = digitalRead(8);  
  int button2Value = digitalRead(9);
  int button3Value = digitalRead(10);

  if (button1Value == LOW && lastButtonState1 == HIGH) {
    if (millis() - lastDebounceTime1 > debounceDelay) {
      button_Switch1 = (button_Switch1 == 0) ? 1 : 0;  
      lastDebounceTime1 = millis();
    }
  }

  if (button2Value == LOW && lastButtonState2 == HIGH) {
    if (millis() - lastDebounceTime2 > debounceDelay) {
      button_Switch2 = (button_Switch2 == 0) ? 1 : 0;  
      lastDebounceTime2 = millis();
    }
  }

  if (button3Value == LOW && lastButtonState3 == HIGH) {
    if (millis() - lastDebounceTime3 > debounceDelay) {
      button_Switch3 = (button_Switch3 == 0) ? 1 : 0;  
      lastDebounceTime3 = millis();
    }
  }

  lastButtonState1 = button1Value;
  lastButtonState2 = button2Value;
  lastButtonState3 = button3Value;

  int sensorValue = analogRead(A0);  
  int audioValue = digitalRead(4);    

  Serial.print(sensorValue);
  Serial.print("/");
  Serial.print(audioValue);
  Serial.print("/");
  Serial.print(button_Switch1);  
  Serial.print("/");
  Serial.print(button_Switch2);  
  Serial.print("/");
  Serial.println(button_Switch3);  

  delay(100);   
}
