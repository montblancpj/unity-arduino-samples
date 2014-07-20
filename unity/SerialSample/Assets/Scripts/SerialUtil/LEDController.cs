using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LEDController {
	static private LEDController s_instance = null;
	private const string LedHeader = "l";
	
	private SerialHandler m_handler = null;
	
	private LEDController()
	{
		m_handler = SerialHandler.GetSerialHandler(ArduinoType.UNO);
	}
	
	static public LEDController Instance 
	{
		get 
		{
			if(s_instance == null)
			{
				s_instance = new LEDController();
			}
			return s_instance;
		}
	}
	
	public void SetLed(bool val) {
		string bright = val ? "254" : "0";
		var data = m_handler.CreateSendData<string>(LedHeader, bright);
		Debug.Log("SetLed" + data);
		m_handler.SendData(data);	
	}
	
	public void Quit()
	{
		if(m_handler != null) {
			m_handler.Stop();	
			m_handler = null;
		}
	}
}
