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
        }

        // 게임 초기 설정 및 메인 메뉴 관리
        class Manager
        {
            private Player player;
            public Manager(Player p)
            {
                player = p;
            }
            public int num;
            public void HelloPlayer()
            {
                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
                Console.Write("그전에, 거기 있는 당신. 이름이 뭔가요?\n>> ");
                player.name = Console.ReadLine();
            }
            public void Start()
            {
                Console.WriteLine("===================================================================");
                Console.WriteLine("반갑습니다. " + player.name + ". 이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");
                Console.WriteLine("1. 상태 보기\n2. 인벤토리\n3. 상점\n");
                Console.Write("원하시는 행동을 입력해주세요\n>> ");
                num = int.Parse(Console.ReadLine());
            }
        }

        
    }
}
