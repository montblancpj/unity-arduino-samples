using UnityEngine;
using System.Collections;

public class ServoController {
	private const string ServoHeader = "s";
	private const int MinDeg = 88;
	private const int MaxDeg = 140;

	static private ServoController s_instance = null;

	
	private SerialHandler m_handler = null;
	
	private ServoController()
	{
		m_handler = SerialHandler.GetSerialHandler(ArduinoType.UNO);
	}
	
	static public ServoController Instance 
	{
		get 
		{
			if(s_instance == null)
			{
				s_instance = new ServoController();
			}
			return s_instance;
		}
	}
	
	public void SetDegree(float degree)
	{
		if(m_handler != null) {
			var data = m_handler.CreateSendData<float>(ServoHeader, degree);
			m_handler.SendData(data);
		}
	}

	/// <summary>
	/// Sets the y.
	/// </summary>
	/// <param name="y">dossun y is between 0(top) and 1(bottom).</param>
	public void SetY(float y)
	{
		if(m_handler != null) {
			if(y >= 0 && y <= 1) {
				int val = MinDeg + (int)(y * (MaxDeg - MinDeg));
				var data = m_handler.CreateSendData<int>(ServoHeader, val);
				m_handler.SendData(data);
			}
		}	
	}
	
	public void Quit()
	{
		if(m_handler != null) {
			m_handler.Stop();	
			m_handler = null;
		}
	}
}
