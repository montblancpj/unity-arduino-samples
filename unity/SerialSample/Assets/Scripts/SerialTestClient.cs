using UnityEngine;
using System.Collections.Generic;

public class SerialTestClient : MonoBehaviour {
	private ServoController   m_servoController;
	private LEDController	  m_ledController;
	private SolenoidController m_solenoidController;
	
	public int m_servoValue = 0;
	private bool servoUp = false;
	private bool ledOn = false;
	private bool solenoidOn = false;

	// Use this for initialization
	void Start () {
		m_servoController = ServoController.Instance;
		m_ledController = LEDController.Instance;
		m_solenoidController = SolenoidController.Instance;
	}
	
	// Update is called once per frame
	void Update () {
	}
				
    void OnGUI() {
		// Servo Update
//		var val = 0;
//        val = (int)GUI.HorizontalSlider(new Rect(10, 10, 200, 40), m_servoValue, 0, 100);
//		if(val != m_servoValue) {
//			m_servoValue = val;
//			if(m_servoController != null) {
//				m_servoController.SetY((int)(float)m_servoValue * 0.01f);	
//			}
//		}
//        GUI.Label(new Rect(220, 10, 100, 40), "Servo: " + m_servoValue.ToString());

		// New Servo Update
		var servoRect = new Rect(10, 10, 50, 40);
		var tmpServo = GUI.Toggle(servoRect, servoUp, "Servo");
		if(servoUp != tmpServo) {
			servoUp = tmpServo;
			if(servoUp)	m_servoController.Up();
			else m_servoController.Down();
		}

		
		// LED Update
		var ledRect = new Rect(10, 50, 50, 50);
		var tmp = GUI.Toggle(ledRect, ledOn, "LED");
		if(ledOn != tmp) {
			Debug.Log(tmp);
			ledOn = tmp;
			m_ledController.SetLed(ledOn);
		}

		// Solenoid Update
		var solenoidRect = new Rect(10, 100, 100, 50);
		var tmpSolenoid = GUI.Toggle(solenoidRect, solenoidOn, "Solenoid");
		if(solenoidOn != tmpSolenoid) {
			Debug.Log(tmpSolenoid);
			solenoidOn = tmpSolenoid;
			m_solenoidController.Push(solenoidOn);
		}
    }
	
	void OnApplicationQuit() 
    {
		if(m_servoController != null) {
			m_servoController.Quit();
		}
		if(m_ledController != null) {
			m_ledController.Quit();
		}
		if(m_solenoidController != null) {
			m_solenoidController.Quit();
		}
    }
}
