using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollSelection : MonoBehaviour
{
    public DatePickerControl datepickerinput;
    public GameObject Cube;

    void OnMouseOver()
    {
        
        if (Input.mouseScrollDelta != Vector2.zero)
        {
            if (Input.mouseScrollDelta.y < 0 )
            {
                if (Cube.gameObject.CompareTag("daycube"))
                {
                    datepickerinput.diaMin();
                    print("You should SUBTRACT here");
                }

              else  if (Cube.gameObject.CompareTag("month"))
                {
                    datepickerinput.mesMin();
                    print("You should SUBTRACT here");
                }


              else  if (Cube.gameObject.CompareTag("year"))
                {
                    datepickerinput.yearMin();
                    print("You should SUBTRACT here");
                }

              else  if (Cube.gameObject.CompareTag("hour"))
                {
                    datepickerinput.hourMin();
                    print("You should SUBTRACT here");
                }
              else  if (Cube.gameObject.CompareTag("minute"))
                {
                    datepickerinput.minuteMin();
                    print("You should SUBTRACT here");
                }
            }

            if (Input.mouseScrollDelta.y > 0)
            {
                if (Cube.gameObject.CompareTag("daycube"))
                {
                    datepickerinput.diaMax();
                    print("You should ADD here");
                }

                else if (Cube.gameObject.CompareTag("month"))
                {
                    datepickerinput.mesMax();
                    print("You should SUBTRACT here");
                }


                else if (Cube.gameObject.CompareTag("year"))
                {
                    datepickerinput.yearMax();
                    print("You should SUBTRACT here");
                }

                else if (Cube.gameObject.CompareTag("hour"))
                {
                    datepickerinput.hourMax();
                    print("You should SUBTRACT here");
                }
                else if (Cube.gameObject.CompareTag("minute"))
                {
                    datepickerinput.minuteMax();
                    print("You should SUBTRACT here");
                }
            }
            // fecha = new DateTime(fecha.Year, fecha.Month, fecha.Day + 1, fecha.Hour, fecha.Minute, fecha.Second);
            //print(Input.mouseScrollDelta);
        }
    }


}