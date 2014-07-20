using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Collections.Generic;
using System.Text;

public enum ArduinoType {
	DUEMILANOVE,
	UNO
}

public class SerialHandler {
	const string UnoPort = "/dev/tty.usbserial-A5005CK9";
//	const string UnoPort = "/dev/tty.usbserial-A800ey7d";
	const int BaudRate 	 = 9600;
	
	static private Dictionary<ArduinoType, SerialHandler> m_handlerDict = new Dictionary<ArduinoType, SerialHandler>();
	
	private SerialPort 		m_serial;
	
	private SerialHandler(ArduinoType type)
	{
		switch(type)
		{
//		case ArduinoType.DUEMILANOVE:
//			Debug.Log("Try to open DUEMILANOVE Port " + DuemilanovePort);
//			m_serial = new SerialPort(DuemilanovePort, BaudRate, Parity.None, 8, StopBits.One);
//			break;
		case ArduinoType.UNO:
			Debug.Log("Try to open UNO Port " + UnoPort);
			m_serial = new SerialPort(UnoPort, BaudRate, Parity.None, 8, StopBits.One);
			break;
		}
		
		Start();
	}
	
	static public SerialHandler GetSerialHandler(ArduinoType type)
	{
		if(type != ArduinoType.DUEMILANOVE && type != ArduinoType.UNO) return null;
		
		SerialHandler handler = null;
		if(!m_handlerDict.ContainsKey(type)) {
			handler = new SerialHandler(type);
			m_handlerDict.Add(type, handler);
		} else {
			handler = m_handlerDict[type];	
		}
	
		return handler;
	}
	
	private void OnSerialDataReceived(object sender, SerialDataReceivedEventArgs e)
	{
		Debug.Log("OnSerialDataReceived");
		SerialPort port = (SerialPort)sender;
		byte[] buf = new byte[1024];
		int len = port.Read(buf, 0, 1024);
		string s = Encoding.GetEncoding("Shift_JIS").GetString(buf, 0, len);
		Debug.Log(s);
	}

	private void OnSerialErrorReceived(object sender, SerialErrorReceivedEventArgs e)
	{
		Debug.Log("Serial port error: " + e.EventType.ToString ("G"));
	}
	
	private void IOErrorHandler()
	{
		Debug.LogError("IOException!!!!");
		Stop();
	}
	
	private void Start() {
		if(m_serial != null) {
			if(m_serial.IsOpen) {
				Debug.LogError("Failed to open Serial Port, already open!");
				m_serial.Close();
			} else {
				try
				{
					m_serial.DataReceived += OnSerialDataReceived;
					m_serial.ErrorReceived += OnSerialErrorReceived;
					m_serial.Open();
					m_serial.DtrEnable = true;
					m_serial.RtsEnable = true;
					m_serial.ReadTimeout = 50;
					Debug.Log("Open Serial port");
				}
				catch(System.IO.IOException)
				{
					IOErrorHandler();
				}
			}
		}
	}
	
	public void Stop() 
    {
		if(m_serial != null) {
			Debug.Log("CLose Serial Port");
			m_serial.Close();
		}
    }
		
	public string CreateSendData<T>(string header, T data)
	{
		return  header + data.ToString() + "\0";
	}
	
	public void SendData(string data)
	{
		try
		{
			Debug.Log(data);
			m_serial.Write(data);
		}
		catch(System.IO.IOException)
		{
			IOErrorHandler();
		}
	}
	
}
