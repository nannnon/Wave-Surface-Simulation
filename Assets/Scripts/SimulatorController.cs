﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulatorController : MonoBehaviour
{
    const int kWidth = WaveSurfaceController.kWidth;
    const int kHeight = WaveSurfaceController.kHeight;

    enum BlockType
    {
        None,
        Free,
        Fixed
    }

    struct Unit
    {
        public float y;
        public float v;
        public BlockType bt;
    }

    //struct WaveSource
    //{
    //    public int u;
    //    public int v;
    //    public float theta;
    //    public float omega;
    //    public float amp;
    //}


    WaveSurfaceController m_wsc;
    Unit[,] m_units;
    //List<WaveSource> m_waveSources;


    // Start is called before the first frame update
    void Start()
    {
        m_wsc = GameObject.Find("WaveSurface").GetComponent<WaveSurfaceController>();
        m_units = new Unit[kWidth, kHeight];

        for (int i = 0; i < kWidth; ++i)
        {
            for (int j = 0; j < kHeight; ++j)
            {
                m_units[i, j].y = 0;
                m_units[i, j].v = 0;
                m_units[i, j].bt = BlockType.None;
            }
        }

        for (int i = 0; i < kWidth; ++i)
        {
            m_units[i,           0].bt = BlockType.Fixed;
            m_units[i, kHeight - 1].bt = BlockType.Fixed;
        }
        for (int j = 0; j < kHeight; ++j)
        {
            m_units[         0, j].bt = BlockType.Fixed;
            m_units[kWidth - 1, j].bt = BlockType.Fixed;
        }


        m_units[kWidth / 2, kHeight / 2].y = 30f;
    }

    void FixedUpdate()
    {
        Move();
        Reflect();
    }

    void Move()
    {
        for (int i = 0; i < kWidth; ++i)
        {
            for (int j = 0; j < kHeight; ++j)
            {
                if (m_units[i, j].bt == BlockType.None)
                {
                    // 減衰
                    const float kF = 0.005f;
                    m_units[i, j].v -= kF * m_units[i, j].v;

                    for (int du = -1; du < 2; ++du)
                    {
                        for (int dv = -1; dv < 2; ++dv)
                        {
                            if (du == 0 && dv == 0)
                            {
                                continue;
                            }

                            int u = i + du;
                            int v = j + dv;

                            if (InRange(u, v) && m_units[u, v].bt != BlockType.Free)
                            {
                                // 上下左右との変位差から加速度を算出して加算
                                if ((du + dv) % 2 != 0)
                                {
                                    const float kK = 0.05f;
                                    m_units[i, j].v += kK * (m_units[u, v].y - m_units[i, j].y);
                                }
                            }
                        }
                    }
                }
            }
        }

        for (int i = 0; i < kWidth; ++i)
        {
            for (int j = 0; j < kHeight; ++j)
            {
                if (m_units[i, j].bt == BlockType.None)
                {
                    m_units[i, j].y += m_units[i, j].v;
                }
            }
        }
    }

    void Reflect()
    {
        for (int i = 0; i < kWidth; ++i)
        {
            for (int j = 0; j < kHeight; ++j)
            {
                Unit unit = m_units[i, j];

                if (unit.bt == BlockType.None)
                {
                    Color color = GetColor((unit.y + 2.0f) / 4.0f);
                    m_wsc.Set(i, j, unit.y, color);
                }
                else if (unit.bt == BlockType.Free)
                {
                    m_wsc.Set(i, j, 0, Color.white);
                }
                else if (unit.bt == BlockType.Fixed)
                {
                    m_wsc.Set(i, j, 0, Color.black);
                }
            }
        }

        m_wsc.Apply();
    }

    bool InRange(int i, int j)
    {
        if (i >= 0 && i < kWidth && j >= 0 && j < kHeight)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    Color GetColor(float value)
    {
        int count = (int)(value * 255.0f);

		if (count > 255)
		{
			count = 255;
		}
		else if (count < 0)
		{
			count = 0;
		}

		int m = (int)(count / 64);
        int r, g, b;

        switch (m)
		{
		case 0:	//青→シアン
			r = 0;                  g = 4 * count;                  b = 255;				    	break;
		case 1:	//シアン→緑
			r = 0;                  g = 255;						b = 255 - 4 * (count - 64 );	break;
		case 2:	//緑→黄
			r = 4 * (count - 128 ); g = 255;						b = 0;							break;
		case 3:	//黄→赤
			r = 255;				g = 255 - 4 * (count - 192 );	b = 0;							break;
		default:
			r = 0;					g = 0;							b = 0;							break;
		}

        return new Color(r / 255f, g / 255f, b / 255f, 1);
    }
}