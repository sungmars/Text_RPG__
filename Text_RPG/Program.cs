using System;
using System.Collections.Generic;

namespace Text_RPG_Console
{
    internal class Text_RPG
    {
        // 플레이어 클래스
        class Player
        {
            public string name { get; set; } = "";
            public int lv { get; set; } = 1;
            public string job { get; set; } = "( 전사 )";
            public int att { get; set; } = 10;
            public int df { get; set; } = 5;
            public int hp { get; set; } = 100;
            public int gold { get; set; } = 1500;  // 초기 골드
            //테스트 커밋
        }

    }
}
