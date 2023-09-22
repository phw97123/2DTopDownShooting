# Steampunk Arrow


B7조 박희원 


스파르타 던전 Unity 버전 만들기 


## 구현 기능


| Class | 기능 |
| :---: | :---: |
| GameManager | [플레이어 정보 UI 연결](https://github.com/phw97123/2DTopDownShooting/blob/9e129fbb7ece084d5c59dc7457562a3a6c2c9bbf/Assets/Scripts/Global/GameManager.cs#L178-L182) |
| GameManager | [플레이어 능력치 UI 연결](https://github.com/phw97123/2DTopDownShooting/blob/9e129fbb7ece084d5c59dc7457562a3a6c2c9bbf/Assets/Scripts/Global/GameManager.cs#L184-L190) |
| PickupStatModifiers | [장착 아이템 줍기](https://github.com/phw97123/2DTopDownShooting/blob/9e129fbb7ece084d5c59dc7457562a3a6c2c9bbf/Assets/Scripts/Items/PickupStatModifiers.cs#L11-L20) |
| ItemSlotUI | [아이템 슬롯에 아이템 정보 저장](https://github.com/phw97123/2DTopDownShooting/blob/9e129fbb7ece084d5c59dc7457562a3a6c2c9bbf/Assets/Scripts/UI/ItemSlotUI.cs#L35-L46) |
| Inventory | [인벤토리 아이템 추가](https://github.com/phw97123/2DTopDownShooting/blob/9e129fbb7ece084d5c59dc7457562a3a6c2c9bbf/Assets/Scripts/Inventory.cs#L59-L69) |
| Inventory | [인벤토리 슬롯 선택 및 장착확인 패널](https://github.com/phw97123/2DTopDownShooting/blob/9e129fbb7ece084d5c59dc7457562a3a6c2c9bbf/Assets/Scripts/Inventory.cs#L101-L117) |
| Inventory | [아이템 장착 및 해제](https://github.com/phw97123/2DTopDownShooting/blob/9e129fbb7ece084d5c59dc7457562a3a6c2c9bbf/Assets/Scripts/Inventory.cs#L119-L150) |
| TooltipController | [아이템 정보 창](https://github.com/phw97123/2DTopDownShooting/blob/9e129fbb7ece084d5c59dc7457562a3a6c2c9bbf/Assets/Scripts/UI/TooltipController.cs#L16-L33) |
| Tooltip | [아이템 정보창 위치 조정](https://github.com/phw97123/2DTopDownShooting/blob/9e129fbb7ece084d5c59dc7457562a3a6c2c9bbf/Assets/Scripts/UI/Tooltip.cs#L20-L28) |



| 기능 | 날짜 | 설명 |
|------|---|---|
| Menu 창 생성 |	 2023.9.19 | TAP 키를 누르면 메뉴창이 나오며 플레이어의 정보, 능력치, 인벤토리를 볼 수 있다 | 
| Player Info 창 생성 및 연결 | 2023.9.19 |	플레이어의 정보를 볼 수 있다 | 
| Player Ability 창 생성 및 연결 | 2023.9.18 |	플레이어의 능력치를 볼 수 있다 (장착된 아이템이 있다면 능력치가 더해진다) |
| 장착 아이템 추가	| 2023.9.20 |	플레이어가 장착할 수 있는 아이템으로 기존에 있는 물약 아이템과는 달리 바로 사용되지 않고 인벤토리로 들어간다 |
| 인벤토리	| 2023.9.20	| 장착 아이템을 먹으면 아이템이 들어가고 장착 및 해제가 가능하다 |
| ToolTip	| 2023.9.20 |	인벤토리에서 비어있지 않은 아이템 슬롯에 마우스를 가져다대면 아이템 설명이 나온다 |
| 장착 아이템 생성 로직 | 2023.9.21 |	전투 중 3의 배수의 Wave 마다 장착 아이템이 생성되게 하였다 |

## 시연영상


https://youtu.be/wxU7j9uUiXs?si=TucUwSMu_-5fBlbB






