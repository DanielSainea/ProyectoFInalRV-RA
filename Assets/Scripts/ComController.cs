/*******
 * DUCK HUNT
 * Version : 1.0 (part 2 of clip)
 * Hoang Minh Quan
 * http://khoahocvui.vn
 * *****/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityEngine.UI;
using System;

public class ComController : MonoBehaviour
{
    public static SerialPort spCom;
    public Dropdown dropBoxPort;
    public Text lbMsg;
    public int readTimeOut = 50; // wait time out : 500ms
   
    // Start is called before the first frame update
    void Start()
    {
        lbMsg.text = "";

        string[] ports = SerialPort.GetPortNames();
        foreach (string port in ports)
        {
            dropBoxPort.options.Add(new Dropdown.OptionData(port));
        }
    }

   private void OnDestroy()
{
    if (spCom != null && spCom.IsOpen)
    {
        spCom.Close();
    }
}
    // Create A Port
    public void CreatePortWithCallback(Action act)
    {
        string value = dropBoxPort.options[dropBoxPort.value].text;
        spCom = new SerialPort(value, 9600 , Parity.None, 8, StopBits.One);
        bool isOK = false;
        if (spCom != null)
        {
            Debug.Log("COM NOW : " + spCom.IsOpen);
            if (!spCom.IsOpen)
            {
               
                try
                {
                    spCom.Open();
                    spCom.ReadTimeout = readTimeOut; // wait time out : 500ms
                    if (spCom.ReadByte() > 0)
                    {
                        isOK = true;
                        act.Invoke();
                        Debug.Log(value + " ABIERTO!");
                    }
                }
              catch (Exception e)
{
    Debug.LogError(e.ToString());
    lbMsg.text = "ERROR DE PUERTO!";
}
            }
        }

        if (!isOK)
            lbMsg.text = "El arma no se ha conectado al puerto";
    }

}
