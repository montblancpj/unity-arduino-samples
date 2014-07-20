using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SolenoidController {
	static private SolenoidController s_instance = null;
	private const string SolenoidHeader = "p";
	
	private SerialHandler m_handler = null;
	
	private SolenoidController()
	{
		m_handler = SerialHandler.GetSerialHandler(ArduinoType.UNO);
	}
	
	static public SolenoidController Instance 
	{
		get 
		{
			if(s_instance == null)
			{
				s_instance = new SolenoidController();
			}
			return s_instance;
		}
	}
	
	public void Push(bool val) {
		var value = val ? 1 : 0;
		var data = m_handler.CreateSendData<int>(SolenoidHeader, value);
		Debug.Log("Push " + data);
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
