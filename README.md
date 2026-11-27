<div dir="rtl">

# השוד המושלם: קוד המשחק | PERFECT HEIST – Code

**קוד המשחק (Unity)** בקורס פיתוח משחקי מחשב. התחיל כאב טיפוס במטלה 4 (תהליך הליבה, פיסיקה וארכיטקטורה, בהמשך לרמת המדריך ממטלה 3), ובמטלה 5 נוסף לו אירוע חי (Live Ops). כל מסמכי העיצוב נמצאים במאגר [Perfect-Heist-Design](https://github.com/Kfir-And-Evyatar-Game-Dev/Perfect-Heist-Design).
**חברי הצוות:** כפיר נזרי, אביתר אהרון.

> **השוד מתחיל בתוכנית טובה, לא במרוץ נגד השעון.**

| | |
|---|---|
| **לשחק בדפדפן** | **[kfir-and-evyatar.itch.io/perfect-heist-prototype](https://kfir-and-evyatar.itch.io/perfect-heist-prototype)** (גם בטלפון, לרוחב) |
| **רכיבים דרמטיים (מטלה 5)** | [dramatic-elements.md (במאגר העיצוב)](https://github.com/Kfir-And-Evyatar-Game-Dev/Perfect-Heist-Design/blob/main/dramatic-elements.md) |
| **לשחק בעלילה (Twine)** | **[גרסת הדפדפן של הסיפור](https://kfir-and-evyatar-game-dev.github.io/Perfect-Heist-Design/twine/perfect-heist-story.html)**, [קובץ המקור](https://github.com/Kfir-And-Evyatar-Game-Dev/Perfect-Heist-Design/blob/main/twine/perfect-heist-story.twee) |
| **אירוע חי (Live Ops) ואיך בודקים אותו** | [מטלה 5: בדיקת האירוע החי](#מטלה-5-בדיקת-האירוע-החי-live-ops) (בהמשך הקובץ) |
| **הרצה, תרשים מחלקות והחלטות ארכיטקטורה** | [מטלה 4: הרצה וארכיטקטורה](#מטלה-4--הרצה-וארכיטקטורה) (בהמשך הקובץ) |
| **תכנון הרמות של המשחק הזה** | [Level_Design.md: השוד המושלם](https://github.com/Kfir-And-Evyatar-Game-Dev/Perfect-Heist-Design/blob/main/Level_Design.md), עם מפות ותרשימים |
| **הגרסה של מטלה 3 (רמת המדריך)** | [itch.io](https://kfir-and-evyatar.itch.io/perfect-heist-guide-level), [סרטון מהטלפון](https://youtu.be/fIYPyt7VN1s) |

### מטלה 6 — פתיחה ומפות

פתחו את `Assets/Scenes/MainMenu.unity` ב־Unity 6000.3.27f1 ולחצו **Play**.

**א1 · תהליך הפתיחה.** בחרו **Tutorial**. המכתב של סופי אלברז מציג את תעלומת היהלום המזויף. **Rehearse** פותח את אימון המוסך, וחץ מונפש מצביע על ההאקר. אחרי בחירת ההאקר והמתגנב, **Start planning** פותח את המפה. ההדרכה מלמדת לתזמן את כיבוי המצלמה, לתכנן איסוף וחילוץ ולצפות בביצוע המשותף בלחיצה על **RUN**.

- ערכו את `Story Brief` ואת `Practice Brief` ב־Inspector של `TeamSelectScreen` בסצנת `TeamSelect`. [תדריך והכוונת הצוות](Assets/Scripts/UI/TeamSelectScreen.cs#L12), [חץ והילה](Assets/Scripts/Tutorial/GuideOverlay.cs#L146), [הדרכת המפה וציר הזמן](Assets/Scripts/Tutorial/TutorialDirector.cs#L22).

**א2 · מפות.** כל ארבע הבחירות זמינות מהתפריט הראשי. בחרו שוד, הוסיפו את גודל הצוות הנדרש ולחצו **Start planning**. לחיצה על דמות פותחת ציר זמן שאפשר לגרור כדי לצפות בסיור השומרים.

| רמה | סצנה | הבדלים במפה |
| --- | --- | --- |
| קלה, חנות תכשיטים | [JewelryStore](Assets/Scenes/JewelryStore.unity) | 2 דמויות, 2 שומרים, 2 מצלמות, מסוף אבטחה ולייזר |
| בינונית, מוזיאון | [Museum](Assets/Scenes/Museum.unity) | 3 דמויות, מצלמה ושומר נוספים במסדרון השירות, קוד המחולק בין המסוף למחסן |
| קשה, בנק | [Bank](Assets/Scenes/Bank.unity) | 4 דמויות, מצלמה רביעית, סיור מהיר יותר במסדרון, דלת כפולה, שני לוחות, סורג וחידת ציורים |

- המפות משתמשות בתוכנית המבנה המשותפת מהשרטוטים של מטלה 4, עם צבע לכל חדר ופריסת עצמים נפרדת לכל קושי. ב־Inspector ערכו את `Layout` ו־`Room Layout` ב־Grid, את רשימת `Fixtures` ואת ההגדרות של המצלמות והסיורים ב־Level. [תפריט](Assets/Scripts/UI/MainMenuScreen.cs#L63), [בניית Tilemap](Assets/Scripts/World/MapBuilder.cs#L212), [עצמים ולייזר מונפש](Assets/Scripts/World/HeistMapFixtures.cs#L21), [יצירת דמויות ואבטחה](Assets/Scripts/World/LevelSpawner.cs#L80), [תזמון הסיור](Assets/Scripts/Actors/Guard.cs#L56).
- דלתות, מסופים, רמזים ולוחות הם עצמי מפה עם אזורי Trigger. תכנון תנועה, תצוגה מקדימה ושוד בסיסי זמינים דרך מנגנון ההרצה הקיים. חוקי הכספת, הלייזר, טיימר ה־55 שניות ויכולות הצוות הייעודיות מתועדים בתכנון הרמות ומיועדים לשלב המשחק המלא.
- ב־**Window → General → Test Runner → PlayMode** הריצו את `AssignmentSixTests`. הבדיקות מכסות את המכתב וההכוונה במשך 30 שניות, ואת המעבר דרך בחירת הצוות לכל אחת משלוש המפות. [בדיקות](Assets/Tests/PlayMode/AssignmentSixTests.cs#L9).

### מטלה 5: בדיקת האירוע החי (Live Ops)

המשחק מוריד בכל הפעלה את קובץ ההגדרות **[liveops/config.json](liveops/config.json)** מ־GitHub Pages ‏([LiveConfigLoader.cs](Assets/Scripts/Game/LiveConfigLoader.cs)). כרגע פעיל בו האירוע **"Faulty Cameras Weekend"**: מערכת המצלמות של "קושובייב אבטחה" קרסה, ולכן המצלמות רואות פחות והפריצה אליהן מחזיקה יותר זמן.

**מה רואים כשהאירוע פעיל:**
1. בתפריט הראשי, מעל שמות היוצרים, מופיע באנר זהוב: `LIVE EVENT: Faulty Cameras Weekend. Cameras see less, hacks last longer!`
2. בשלב ההדרכה, העיגול האדום של המצלמה קטן מ־2 תאים לתא אחד (`cameraRadiusMultiplier: 0.5`).
3. הבלוק "Camera off" בציר הזמן של ההאקר נמשך 12 שניות במקום 8 (`cameraOffBonusSeconds: 4`).

**מה משנים בקובץ:**

| מה רוצים | מה משנים ב־`liveops/config.json` | מה רואים |
|---|---|---|
| לכבות את האירוע | `"forceActive": false` (חלון התאריכים של האירוע הוא 4–7.6.2027, כך שעד אז הוא לא יופעל לבד) | הבאנר נעלם, הרדיוס חוזר ל־2, "Camera off" חוזר ל־8 שניות |
| לכבות לגמרי | `"eventId": ""` | אותו דבר, בלי קשר לתאריכים |
| מצלמות חזקות במקום חלשות | `"cameraRadiusMultiplier": 1.5` | העיגול האדום גדל ל־3 תאים |
| הפעלה רק לפי תאריך | `"forceActive": false` ותאריכים אחרים ב־`startUtc` / `endUtc` (UTC), למשל תאריך של היום | האירוע מופיע רק בטווח התאריכים |

**דרך 1: בדיקה עצמאית ב־Unity (לא צריך הרשאות לריפו).**
1. מורידים את הריפו (`git clone` או Code ← Download ZIP) ופותחים אותו ב־Unity 6000.3.27f1.
2. פותחים את הסצנה `Assets/Scenes/MainMenu.unity` ולוחצים **Play**: הבאנר מופיע, וב־Console כתוב `Live config v1 loaded. Event 'faulty-cameras-weekend' active: True.`
3. עוצרים את Play, פותחים בעורך טקסט את `liveops/config.json` (בתיקייה הראשית של הריפו, לא בתוך Assets) ומשנים ערך מהטבלה, למשל `"forceActive": false`. שומרים.
4. לוחצים שוב **Play**: ב־Console כתוב `active: False`, הבאנר לא מופיע, ובהדרכה המצלמה חוזרת לגודל הרגיל.

בעורך המשחק קורא את הקובץ ישירות מהתיקייה (`LiveConfigLoader.UseLocalFileInEditor`), ולכן אין צורך ב־push או ב־build. זה אותו קוד שרץ ב־itch, רק שהקובץ מגיע מהדיסק ולא מהאינטרנט.

**דרך 2: בגרסה החיה ב־itch (למי שיש הרשאת כתיבה לריפו).**
משנים את `liveops/config.json` ב־GitHub (אפשר גם בעריכה ישירה באתר, בעיפרון), עושים commit, מחכים דקה־שתיים ש־GitHub Pages יתעדכן ומרעננים את [דף המשחק ב־itch](https://kfir-and-evyatar.itch.io/perfect-heist-prototype). המשחק מוסיף לכתובת `?t=<זמן>` כדי לא לקבל עותק ישן. אם ההורדה נכשלת, המשחק ממשיך בלי אירוע וכותב אזהרה ל־Console. אם תרצו לראות את זה בלי הרשאות, כתבו לנו ונכבה ונדליק את האירוע בזמן שאתם בודקים.

### מטלה 4: הפיסיקה בשלושת המשחקים

כל אחת מהתכונות שנלמדו מופיעה לפחות באחד מהמשחקים. **CharacterController** ממומש כאן, כי ל-`CharacterController` אין גרסה דו־ממדית ושלושת המשחקים שלנו דו־ממדיים.

| תכונה | איפה |
|---|---|
| **CharacterController** | **השוד המושלם:** המתגנב זז ב-`CharacterController.Move` מול `BoxCollider` תלת־ממדי על הקירות. אם הוא נתקע בקיר, התוכנית נכשלת ([`Character.cs`](Assets/Scripts/Actors/Character.cs), [`MapBuilder.cs`](Assets/Scripts/World/MapBuilder.cs)) |
| **Rigidbody2D** | השוד המושלם (דמויות Kinematic שזזות ב-`MovePosition` כדי שהטריגרים יזהו מגע), [כיבוש המפה](https://github.com/Kfir-And-Evyatar-Game-Dev/Map-Conquest-Prototype), [רעש שמציל](https://github.com/Kfir-And-Evyatar-Game-Dev/Sound-of-Survival-prototype) |
| **AddForce** | [כיבוש המפה](https://github.com/Kfir-And-Evyatar-Game-Dev/Map-Conquest-Prototype): שיגור פגזים והדיפה |
| **Raycast** | [כיבוש המפה](https://github.com/Kfir-And-Evyatar-Game-Dev/Map-Conquest-Prototype): בחירת יחידה ואזור בעכבר |
| **Collision2D** | [כיבוש המפה](https://github.com/Kfir-And-Evyatar-Game-Dev/Map-Conquest-Prototype), [רעש שמציל](https://github.com/Kfir-And-Evyatar-Game-Dev/Sound-of-Survival-prototype) |

### הריפוזיטוריז של המשחקים
| משחק | קוד | עיצוב |
|---|---|---|
| השוד המושלם | הריפו הזה | [Perfect-Heist-Design](https://github.com/Kfir-And-Evyatar-Game-Dev/Perfect-Heist-Design) |
| כיבוש המפה | [Map-Conquest-Prototype](https://github.com/Kfir-And-Evyatar-Game-Dev/Map-Conquest-Prototype) | [Map-Conquest](https://github.com/Kfir-And-Evyatar-Game-Dev/Map-Conquest) |
| רעש שמציל | [Sound-of-Survival-prototype](https://github.com/Kfir-And-Evyatar-Game-Dev/Sound-of-Survival-prototype) | [Sound-of-Survival](https://github.com/Kfir-And-Evyatar-Game-Dev/Sound-of-Survival) |

---

## על המשחק

"השוד המושלם" הוא משחק תכנון דו־ממדי במבט מלמעלה. בוחרים צוות שודדים, נותנים לכל דמות פקודות על **ציר זמן**, ורק כשהתוכנית מוכנה לוחצים **RUN** וצופים בדמויות מבצעות אותה לבד. **אין הגבלת זמן לתכנון.** אם השוד נכשל, מקבלים הסבר מה השתבש, והתוכנית נשמרת כדי לתקן אותה.

**רמת המדריך** מלמדת את המשחק צעד אחר צעד, ברמת פירוט שמתאימה גם למי שלא שיחק אף פעם: בכל צעד מסומן בהילה הדבר היחיד שאפשר ללחוץ עליו, עם הסבר קצר מה לעשות.

<img src="Assets/Resources/Portraits/Hacker.png" height="110"> <img src="Assets/Resources/Portraits/Sneak.png" height="110"> <img src="Assets/Resources/Portraits/Guard.png" height="110"> <img src="Assets/Resources/Portraits/Diamond.png" height="90">

## מהלך המשחק

1. **דף הבית:** בוחרים שוד. אפשר לבחור במדריך, בחנות התכשיטים, במוזיאון או בבנק.
2. **בחירת צוות:** חמש דמויות. לחיצה על דמות מציגה את היכולות שלה. במדריך בוחרים את **ההאקר** ואת **המתגנב**. שאר הדמויות מוצגות כצלליות נעולות.
3. **מפת התכנון:** רואים את השומר ואת מסלול הסיור שלו, את המצלמה ואת טווח הראייה שלה, את היהלום ואת רכב המילוט.
   - **לחיצה על דמות** פותחת את חלון ציר הזמן שלה, עם הפעולות שלה בלבד: הליכה, המתנה והרמה לכולם, ו־**כיבוי מצלמה** להאקר בלבד.
   - **ציר הזמן** מציג גם את השורות של שאר הדמויות, כדי לתאם תזמון.
4. **RUN:** הדמויות מבצעות את התוכנית. מי שמסיים את התוכנית ליד הרכב נכנס אליו, ואחרי שכולם בפנים עם היהלום הרכב נוסע ממסך המפה.
5. **סיום:**
   - **הצלחה:** עוברים למסך ניצחון, עם זמן ההרצה ומספר הניסיונות.
   - **כישלון:** חלון "Attempt failed" עם הסיבה, למשל "Sneak was seen by the guard at second 13.0", וחזרה לתכנון.

**שליטה:** לחיצה או הקשה בלבד, אותו קוד לעכבר ולמגע. המשחק מוצג לרוחב (Landscape).

---

## מטלה 3: ארבעת הנושאים, ואיפה הם בקוד

### 1. דגמים (Prefabs) ויצירה אוטומטית (Spawn), כולל דגמים מבינה מלאכותית
- **5 Prefabs** בתיקייה `Assets/Prefabs`: ‏Character, ‏Guard, ‏SecurityCamera, ‏Diamond ו־Exit (רכב המילוט).
- **[`LevelSpawner.cs`](Assets/Scripts/World/LevelSpawner.cs)** קורא את המפה, שכתובה כטקסט באותה צורה כמו במסמך התכנון, ויוצר עותק של ה־Prefab המתאים לכל תו עם `Instantiate`:

  | תו | נוצר |
  |---|---|
  | `&` | מצלמה |
  | `1` | שומר |
  | `$` | יהלום |
  | `X` | רכב מילוט |
  | `P` | הצוות שנבחר |

- **דגמים מ־AI:** את הדמויות, השומר, המצלמה, היהלום והרכב יצרנו ב־**Gemini**. הסרנו את הרקע וחתכנו כל אחד לקובץ PNG שקוף.
- גם המפה עצמה נבנית מקוד: [`MapBuilder.cs`](Assets/Scripts/World/MapBuilder.cs) מצייר רצפה וקירות דקים מאותה טבלת טקסט.

### 2. התנגשויות (דו־ממד)
- **דמויות:** ‏`Rigidbody2D` מסוג Kinematic ו־`CircleCollider2D`. הן זזות עם `MovePosition`, כדי שהפיזיקה תזהה מגע.
- **Triggers:**

  | אובייקט | מה קורה במגע | קוד |
  |---|---|---|
  | **מצלמה** | ‏`OnTriggerStay2D`: דמות בתוך הטווח, כשהמצלמה פעילה, מתגלה | [`SecurityCamera.cs`](Assets/Scripts/Actors/SecurityCamera.cs) |
  | **יהלום** | הדמות יודעת שהיא עומדת עליו ויכולה להרים אותו | [`Character.cs`](Assets/Scripts/Actors/Character.cs), [`Diamond.cs`](Assets/Scripts/World/Diamond.cs) |
  | **רכב המילוט** | דמות שסיימה את התוכנית בתוך הטריגר נכנסת לרכב תוך חצי שנייה, ואחר כך הרכב נוסע | [`ExitZone.cs`](Assets/Scripts/World/ExitZone.cs) |

- **השומר:** רואה 3 משבצות קדימה בזווית של 90°, ולא רואה דרך קירות ([`Guard.cs`](Assets/Scripts/Actors/Guard.cs)).

### 3. ניהול רמות ושמירת ערכי משתנים
- **[`GameManager.cs`](Assets/Scripts/Game/GameManager.cs):** ‏**Singleton** עם **`DontDestroyOnLoad`**. הוא עובר בין הסצנות ושומר את השוד שנבחר, את הצוות, את מספר הניסיונות ואת זמן ההרצה. גם "המדריך הושלם" נשמר במכשיר עם `PlayerPrefs`.
- **ארבע סצנות:** ‏`MainMenu`, ‏`TeamSelect`, ‏`Tutorial` ו־`Win`.
- **מסך ניצחון** ([`WinScreen.cs`](Assets/Scripts/UI/WinScreen.cs)) מציג את הערכים שנשמרו ב־GameManager.
- **חלון כישלון** מחזיר לתכנון, והתוכנית נשמרת.

### 4. התאמה לטלפונים ניידים
- **מסך לרוחב בלבד.**
- **אותו קוד לעכבר ולמגע:** אירועי UI, בלי מקשים.
- **כפתורים גדולים:** ממשק בקנה מידה של 1920×1080 שמתאים את עצמו לכל מסך.
- **[`CameraFit.cs`](Assets/Scripts/UI/CameraFit.cs)** מתאים את הזום של המצלמה כך שהמפה נכנסת לכל יחס מסך.
- **בנייה ל־WebGL** (itch.io, בדפדפן של הטלפון) **ול־Android** (APK).

---

## מבנה הקוד

| קובץ | תפקיד |
|---|---|
| `RunClock.cs` | שעון ההרצה **הדטרמיניסטי**: אותה תוכנית תמיד נותנת אותה תוצאה. מחליט על הצלחה או כישלון |
| `Commands.cs` | פקודות ציר הזמן: הליכה (מסלול BFS מסביב לקירות), המתנה, כיבוי מצלמה והרמה |
| `PlanningUI.cs` | מסך התכנון: לחיצה על דמות, חלון ציר הזמן, כפתורי הפעולות וחלון הכישלון |
| `TutorialDirector.cs` | שלבי המדריך במפה (כ־40 צעדים) |
| `GuideOverlay.cs`, `GuideRunner.cs` | מערכת ההדרכה: החשכה, הילה, חץ, בועת טקסט, וחסימת לחיצות מחוץ להילה |
| `MainMenuScreen.cs`, `TeamSelectScreen.cs`, `WinScreen.cs` | המסכים. הממשק נבנה בקוד, ונוצר אוטומטית עם `ScreenBootstrap.cs` |
| `UIKit.cs`, `CharacterCatalog.cs` | עזרים לבניית ממשק, ונתוני הדמויות |

---

## עזרה וכלי בינה מלאכותית

| כלי | למה השתמשנו בו |
|---|---|
| **Claude Code** (Anthropic) | כתיבת הקוד יחד איתנו, תכנון הארכיטקטורה (שעון הרצה דטרמיניסטי, פקודות, מערכת ההדרכה), ועיבוד התמונות: הסרת רקע מזויף וחיתוך לקבצים נפרדים |
| **Gemini** (Google) | יצירת התמונות של הדמויות, השומר, המצלמה, היהלום ורכב המילוט |

</div>

## מטלה 4 — הרצה וארכיטקטורה

פתחו את הפרויקט ב־Unity 6000.3.27f1, פתחו את הסצנה `Assets/Scenes/MainMenu.unity` ולחצו **Play**. בחרו במדריך והתקדמו עד לתכנון הצוות. אחרי שהמדריך מאפשר להריץ את התוכנית, אפשר לגרור על ציר הזמן לתצוגה מקדימה. לחצו **RUN** לביצוע. כישלון מחזיר לתכנון עם הפקודות שהוזנו, והצלחה מעבירה למסך הניצחון.

תיקיית `Actors` כוללת את הדמויות והאבטחה, `Game` את מצב המשחק וההרצה, `Planning` את פקודות ציר הזמן, `World` את המפה ואת העצמים בשלב, `UI` את המסכים והתצוגה המקדימה, ו־`Tutorial` את ההדרכה. התיקיות נמצאות תחת `Assets/Scripts`.

```mermaid
classDiagram
    LevelSpawner --> MapBuilder
    LevelSpawner --> Character
    LevelSpawner --> Guard
    LevelSpawner --> SecurityCamera
    LevelSpawner --> RunClock : מוסר את אובייקטי השלב
    Character --> Command : מכין ומבצע פקודות
    RunClock --> Character : מקדם את ההרצה
    RunClock --> Guard : בודק זיהוי
    SecurityCamera --> RunClock : מדווחת על זיהוי
    RunClock ..> PlanningUI : OnRunEnded
    PlanningUI --> PlanningMapPreview : מעדכן מסלולים וסימונים
    PlanningUI --> GameManager : מעבר למסך ניצחון
    TutorialDirector --> PlanningUI : מנחה פעולות
```

- בחרנו באירוע `OnRunEnded` במקום קריאה ישירה למסך מתוך `RunClock`. כך ההרצה מדווחת הצלחה או כישלון בלי להכיר את הממשק. `PlanningUI` מפסיק להאזין ב־`OnDisable` ונרשם מחדש כשהוא מופעל.
- שמרנו את הפקודות כטיפוסים נפרדים. החלופה הייתה מחלקה אחת עם תנאי לכל סוג פעולה. לכל פקודה יש חישוב משך וביצוע משלה, וציר הזמן משתמש בממשק המשותף של `Command`.
- הפרדנו את ציור המסלולים והסימונים ל־`PlanningMapPreview`. כך שינוי תצוגת הסיור או סימוני המפה מרוכז במחלקה שמציירת אותם.

מהירות הליכה, זמן כניסה לרכב, מידות בקר המתגנב ומרווח הסיום ניתנים לכוונון ב־Inspector. במהלך הרצה המתגנב משתמש ב־`CharacterController.Move` מול `BoxCollider` תלת־ממדי על קירות המדריך. איסוף היהלום והמילוט מזוהים באמצעות הטריגרים הדו־ממדיים.

`GameManager.Instance` שומר צוות ותוצאות במעבר בין הסצנות. `RunClock.Instance` מאפשר למצלמות של השלב לקרוא זמן ולדווח זיהוי לאותה הרצה. הרישום הסטטי של `SecurityCamera` מאפשר לפקודת הכיבוי למצוא את המצלמה לפי משבצת. מצלמות יוצאות מהרישום כשהן מושבתות. הקטלוגים הסטטיים מרכזים את אפשרויות הצוות והשלבים, ו־`ScreenBootstrap` משתמש באירוע טעינת סצנה כדי לטעון את המסך המתאים.

כדי להוסיף שומר עם סיור שונה, עורכים את הגדרות הסיור ב־`LevelSpawner`. סוג אויב עם כללי זיהוי חדשים דורש סקריפט ו־Prefab משלו וחיבור למנגנון הדיווח של `RunClock`. כך מסך הכישלון יכול לקבל את אותה הודעה דרך האירוע הקיים. לחפץ חדש מוסיפים Prefab וסימון במפה, ומעדכנים את יצירתו ב־`LevelSpawner`. אם החפץ נדרש לניצחון, מעדכנים גם את כלל הניצחון ב־`RunClock`.

### בדיקת התאמה בין התרשים לקוד

המימוש מרחיב את התרשים הראשוני במחלקת `PlanningMapPreview`, שאליה הועבר ציור המסלולים והסימונים מתוך `PlanningUI`. התרשים שלמעלה מתעד את החיבור החדש ואת האירוע שהממשק מקבל בסיום הרצה.
