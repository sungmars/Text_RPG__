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
            public int num;
            public Manager(Player p)
            {
                player = p;
            }
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
                Console.WriteLine("1. 상태 보기\n2. 인벤토리\n3. 상점\n4. 던전입장\n");
                Console.Write("원하시는 행동을 입력해주세요\n>> ");
                num = int.Parse(Console.ReadLine());
            }
        }

        // 상태창 클래스 (플레이어의 능력치 표시)
        class Status
        {
            private Player player;
            private Inventory inventory;
            public int num;
            public Status(Player p, Inventory I)
            {
                player = p;
                inventory = I;
            }
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

        // 인벤토리 클래스 (아이템 관리)
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
            // 상점 혹은 던전 클리어 시 인벤토리에 아이템 추가
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

        // 던전 클래스 (던전 입장 및 진행)
        class Dungeon
        {
            private Player player;
            private Inventory inventory;
            private Random rand;
            public Dungeon(Player p, Inventory inv)
            {
                player = p;
                inventory = inv;
                rand = new Random();
            }
            public void DungeonManager()
            {
                while (true)
                {
                    Console.WriteLine("===================================================================");
                    Console.WriteLine("**던전입장**");
                    Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");
                    Console.WriteLine("1. 쉬운 던전     | 방어력 5 이상 권장");
                    Console.WriteLine("2. 일반 던전     | 방어력 11 이상 권장");
                    Console.WriteLine("3. 어려운 던전    | 방어력 17 이상 권장");
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
                    int recommendedDefense = 0;
                    int baseReward = 0;
                    string dungeonName = "";
                    switch (choice)
                    {
                        case 1:
                            recommendedDefense = 5;
                            baseReward = 1000;
                            dungeonName = "쉬운 던전";
                            break;
                        case 2:
                            recommendedDefense = 11;
                            baseReward = 1700;
                            dungeonName = "일반 던전";
                            break;
                        case 3:
                            recommendedDefense = 17;
                            baseReward = 2500;
                            dungeonName = "어려운 던전";
                            break;
                        default:
                            Console.WriteLine("잘못된 입력입니다.");
                            continue;
                    }

                    // 내 총 방어력과 공격력 (기본 능력치 + 인벤토리 보너스)
                    int bonusDf = inventory.Dfup();
                    int totalDefense = player.df + bonusDf;
                    int bonusAtt = inventory.Attup();
                    int totalAttack = player.att + bonusAtt;

                    Console.WriteLine("===================================================================");
                    Console.WriteLine($"던전 도전 : {dungeonName}");
                    Console.WriteLine($"추천 방어력 : {recommendedDefense} | 내 방어력 : {totalDefense}");

                    bool dungeonFailed = false;
                    // 내 방어력이 추천치 미만인 경우 40% 실패 확률 적용
                    if (totalDefense < recommendedDefense)
                    {
                        int chance = rand.Next(100); // 0 ~ 99
                        if (chance < 40)
                        {
                            dungeonFailed = true;
                        }
                    }

                    // 기본 체력 소모량 : 20 ~ 35 랜덤
                    int baseDamage = rand.Next(20, 36);
                    // 조정값: (추천 방어력 - 내 방어력)
                    int damageAdjustment = recommendedDefense - totalDefense;
                    int totalDamage = baseDamage + damageAdjustment;
                    totalDamage = Math.Max(0, totalDamage);
                    if (dungeonFailed)
                    {
                        // 실패 시 체력 소모는 계산된 데미지의 절반
                        totalDamage = totalDamage / 2;
                    }

                    int initialHP = player.hp;
                    player.hp -= totalDamage;

                    int initialGold = player.gold;
                    int reward = 0;
                    if (!dungeonFailed)
                    {
                        // 추가 보상: 기본 보상의 (내 총 공격력 ~ 내 총 공격력*2)% 범위 내 랜덤 퍼센트 적용
                        int bonusPercentage = rand.Next(totalAttack, totalAttack * 2 + 1);
                        int additionalReward = baseReward * bonusPercentage / 100;
                        reward = baseReward + additionalReward;
                        player.gold += reward;
                    }

                    Console.WriteLine();
                    if (dungeonFailed)
                    {
                        Console.WriteLine("**던전 실패**");
                        Console.WriteLine("아쉽게도 던전 도중에 실패하였습니다.");
                    }
                    else
                    {
                        Console.WriteLine("**던전 클리어**");
                        Console.WriteLine("축하합니다!!");
                        Console.WriteLine($"{dungeonName}을(를) 클리어 하였습니다.");
                    }
                    Console.WriteLine();
                    Console.WriteLine("[탐험 결과]");
                    Console.WriteLine($"체력 {initialHP} -> {player.hp}");
                    if (!dungeonFailed)
                    {
                        Console.WriteLine($"Gold {initialGold} G -> {player.gold} G ");
                    }
                    else
                    {
                        Console.WriteLine("Gold 변화 없음");
                    }
                    Console.WriteLine("\n0. 나가기");
                    Console.Write("원하시는 행동을 입력해주세요.\n>> ");
                    int exitInput = int.Parse(Console.ReadLine());
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
                        Shop shop = new Shop(player, I);
                        shop.ShopManager();
                        break;
                    case 4:
                        Dungeon dungeon = new Dungeon(player, I);
                        dungeon.DungeonManager();
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        continue;
                }
            }
        }
    }
}
