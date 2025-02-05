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

        // 상태창 (플레이어의 능력치 등 표시)
        class Status
        {
            private Player player;
            private Inventory inventory;
            public Status(Player p, Inventory I)
            {
                player = p;
                inventory = I;
            }
            public int num;
            public void status()
            {
                int bonusAtt = inventory.Attup();
                int bonusDf = inventory.Dfup();
                int totalAtt = player.att + bonusAtt;
                int totalDf = player.df + bonusDf;
                string attBonusDisplay = bonusAtt != 0 ? " (+" + bonusAtt + ")" : "";
                string dfBonusDisplay = bonusDf != 0 ? " (+" + bonusDf + ")" : "";
                Console.WriteLine("===================================================================");
                Console.WriteLine("<<상태보기>>");
                Console.WriteLine("캐릭터의 정보가 표시됩니다\n");
                Console.WriteLine("Lv. " + player.lv);
                Console.WriteLine(player.name + " " + player.job);
                Console.WriteLine("공격력 : " + totalAtt + attBonusDisplay);
                Console.WriteLine("방어력 : " + totalDf + dfBonusDisplay);
                Console.WriteLine("체력 : " + player.hp);
                Console.WriteLine("Gold : " + player.gold + " G\n");
                Console.WriteLine("0. 나가기\n");
                Console.Write("원하시는 행동을 입력해주세요\n>> ");
                num = int.Parse(Console.ReadLine());
            }
        }

        // 인벤토리 클래스 (기존 아이템 목록 + 장착/해제 기능)
        class Inventory
        {
            public string[] items = { };
            public bool[] isEquipped;
            public int num;
            public int num2;
            public int i;
            public string equippedStr;
            public Inventory()
            {
                isEquipped = new bool[items.Length];
            }
            public void Showinven()
            {
                Console.WriteLine("===================================================================");
                Console.WriteLine("<<인벤토리>>\n보유중인 아이템을 관리할 수 있습니다.\n");
                Console.WriteLine("[아이템 목록]\n");
                for (i = 0; i < items.Length; i++)
                {
                    equippedStr = isEquipped[i] ? "[E]" : "";
                    Console.WriteLine($"- {equippedStr}{items[i].Split('|')[0].Trim()}");
                }
                Console.WriteLine("\n1. 장착관리\n2. 나가기\n");
                Console.Write("원하시는 행동을 입력해주세요.\n>> ");
                num = int.Parse(Console.ReadLine());
            }
            public void Equipment()
            {
                while (true)
                {
                    Console.WriteLine("===================================================================");
                    Console.WriteLine("<<인벤토리>> - 장착관리\n보유중인 아이템을 관리할 수 있습니다.\n");
                    Console.WriteLine("[아이템 목록]");
                    for (i = 0; i < items.Length; i++)
                    {
                        equippedStr = isEquipped[i] ? "[E]" : "";
                        Console.WriteLine($"- {i + 1}. {equippedStr} {items[i]}");
                    }
                    Console.WriteLine("\n0. 나가기\n");
                    Console.Write("원하시는 행동을 입력해주세요.\n>> ");
                    num2 = int.Parse(Console.ReadLine());
                    if (num2 == 0)
                    {
                        break;
                    }
                    else
                    {
                        Equipped(num2);
                    }
                }
            }
            public void Equipped(int itemnums)
            {
                int index = itemnums - 1;
                if (index < 0 || index >= items.Length)
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }
                else
                {
                    // 이미 장착중이면 해제, 아니면 장착
                    isEquipped[index] = !isEquipped[index];
                }
            }
            public int Attup()
            {
                int attup = 0;
                for (int i = 0; i < items.Length; i++)
                {
                    if (isEquipped[i])
                    {
                        string effect = items[i].Split('|')[1].Trim();
                        if (effect.StartsWith("공격력"))
                        {
                            string[] parts = effect.Split(' ');
                            if (parts.Length >= 2)
                            {
                                int bonus = int.Parse(parts[1].Replace("+", ""));
                                attup += bonus;
                            }
                        }
                    }
                }
                return attup;
            }
            public int Dfup()
            {
                int dfup = 0;
                for (int i = 0; i < items.Length; i++)
                {
                    if (isEquipped[i])
                    {
                        string effect = items[i].Split('|')[1].Trim();
                        if (effect.StartsWith("방어력"))
                        {
                            string[] parts = effect.Split(' ');
                            if (parts.Length >= 2)
                            {
                                int bonus = int.Parse(parts[1].Replace("+", ""));
                                dfup += bonus;
                            }
                        }
                    }
                }
                return dfup;
            }
            // 상점에서 아이템 구매 시 인벤토리에 추가하기 위한 메서드
            public void AddItem(string newItem)
            {
                List<string> temp = new List<string>(items);
                temp.Add(newItem);
                items = temp.ToArray();

                bool[] tempEq = new bool[isEquipped.Length + 1];
                for (int i = 0; i < isEquipped.Length; i++)
                {
                    tempEq[i] = isEquipped[i];
                }
                tempEq[tempEq.Length - 1] = false;
                isEquipped = tempEq;
            }
        }
        //라라라
        // 상점 아이템의 정보를 담는 클래스
        class ShopItem
        {
            public string Name { get; set; }
            public string Effect { get; set; }
            public string Description { get; set; }
            public int Price { get; set; }
            public bool IsPurchased { get; set; }
        }

        // 상점 클래스
        class Shop
        {
            private Player player;
            private Inventory inventory;
            private List<ShopItem> shopItems;
            public Shop(Player p, Inventory inv)
            {
                player = p;
                inventory = inv;
                // 상점에 등록될 아이템 목록 
                shopItems = new List<ShopItem>()
                {
                    new ShopItem { Name = "수련자 갑옷",    Effect = "방어력 +5",  Description = "수련에 도움을 주는 갑옷입니다.", Price = 1000, IsPurchased = false },
                    new ShopItem { Name = "무쇠갑옷",      Effect = "방어력 +9",  Description = "무쇠로 만들어져 튼튼한 갑옷입니다.", Price = 1500,    IsPurchased = false  },
                    new ShopItem { Name = "스파르타의 갑옷", Effect = "방어력 +15", Description = "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", Price = 3500, IsPurchased = false },
                    new ShopItem { Name = "낡은 검",      Effect = "공격력 +2",  Description = "쉽게 볼 수 있는 낡은 검 입니다.", Price = 600,  IsPurchased = false },
                    new ShopItem { Name = "청동 도끼",     Effect = "공격력 +5",  Description = "어디선가 사용됐던거 같은 도끼입니다.", Price = 1500, IsPurchased = false },
                    new ShopItem { Name = "스파르타의 창",  Effect = "공격력 +7",  Description = "스파르타의 전사들이 사용했다는 전설의 창입니다.", Price = 3500,    IsPurchased = false  },
                    //추가된 아이템
                    new ShopItem { Name = "사람을 찢는 북극곰의 발톱단검", Effect = "공격력+10", Description = "북극에 산다는 곰의 발톱으로 만든 단검이다.",Price=4500, IsPurchased = false},
                    new ShopItem { Name = "수상하게 튼튼한 녹색 슬리퍼",      Effect = "방어력 +20",  Description = "수상하게 튼튼한 보급형 녹색 슬리퍼다.", Price = 4500,    IsPurchased = false  }
                };
            }

            // 상점 메인 화면
            public void ShopManager()
            {
                while (true)
                {
                    Console.WriteLine("===================================================================");
                    Console.WriteLine("<<상점>>");
                    Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
                    Console.WriteLine("[보유 골드]");
                    Console.WriteLine(player.gold + " G\n");
                    Console.WriteLine("[아이템 목록]");
                    foreach (var item in shopItems)
                    {
                        string priceDisplay = item.IsPurchased ? "구매완료" : (item.Price + " G");
                        Console.WriteLine($"- {item.Name,-15} | {item.Effect,-10} | {item.Description,-45} | {priceDisplay}");
                    }
                    Console.WriteLine("\n1. 아이템 구매");
                    Console.WriteLine("0. 나가기");
                    Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
                    int choice;
                    if (!int.TryParse(Console.ReadLine(), out choice))
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        continue;
                    }
                    if (choice == 0)
                    {
                        break;
                    }
                    else if (choice == 1)
                    {
                        PurchaseItem();
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                }
            }

            // 아이템 구매 메뉴
            private void PurchaseItem()
            {
                while (true)
                {
                    Console.WriteLine("===================================================================");
                    Console.WriteLine("<<상점 - 아이템 구매>>");
                    Console.WriteLine("[아이템 목록]");
                    Console.WriteLine("[보유 골드]");
                    Console.WriteLine(player.gold + " G\n");
                    for (int i = 0; i < shopItems.Count; i++)
                    {
                        var item = shopItems[i];
                        string priceDisplay = item.IsPurchased ? "구매완료" : (item.Price + " G");
                        Console.WriteLine($"- {i + 1}. {item.Name,-15} | {item.Effect,-10} | {item.Description,-45} | {priceDisplay}");
                    }
                    Console.WriteLine("0. 나가기");
                    Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
                    int itemChoice;
                    if (!int.TryParse(Console.ReadLine(), out itemChoice))
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        continue;
                    }
                    if (itemChoice == 0)
                    {
                        break;
                    }
                    if (itemChoice < 1 || itemChoice > shopItems.Count)
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        continue;
                    }
                    ShopItem selectedItem = shopItems[itemChoice - 1];
                    if (selectedItem.IsPurchased)
                    {
                        Console.WriteLine("이미 구매한 아이템입니다.");
                    }
                    else
                    {
                        if (player.gold >= selectedItem.Price)
                        {
                            player.gold -= selectedItem.Price;
                            selectedItem.IsPurchased = true;
                            // 구매한 아이템을 인벤토리에 추가 (표시용 문자열)
                            string newItemStr = $"{selectedItem.Name} | {selectedItem.Effect} | {selectedItem.Description}";
                            inventory.AddItem(newItemStr);
                            Console.WriteLine("구매를 완료했습니다.");
                        }
                        else
                        {
                            Console.WriteLine("Gold 가 부족합니다.");
                        }
                    }
                }
            }
        }

        // 메인 메서드
        static void Main(string[] args)
        {
            Player player = new Player();
            Inventory I = new Inventory();
            Manager m = new Manager(player);
            Status s = new Status(player, I);

            m.HelloPlayer();
            while (true)
            {
                m.Start();
                switch (m.num)
                {
                    case 1:
                        while (true)
                        {
                            s.status();
                            switch (s.num)
                            {
                                case 0:
                                    goto EndStatusLoop;
                                default:
                                    Console.WriteLine("잘못된 입력입니다.");
                                    continue;
                            }
                        }
                    EndStatusLoop:
                        break;
                    case 2:
                        while (true)
                        {
                            I.Showinven();
                            switch (I.num)
                            {
                                case 1:
                                    I.Equipment();
                                    break;
                                case 2:
                                    goto EndInventoryLoop;
                                default:
                                    Console.WriteLine("잘못된 입력입니다.");
                                    continue;
                            }
                        }
                    EndInventoryLoop:
                        break;
                    case 3:
                        // 상점 실행
                        Shop shop = new Shop(player, I);
                        shop.ShopManager();
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        continue;
                }
            }
        }
    }
}
