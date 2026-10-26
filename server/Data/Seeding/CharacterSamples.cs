using Miru.Enums;
using Miru.Models;

namespace Miru.Data.Seeding;

public class CharacterSamples
{
    public static List<Character> CreateAll()
    {
        return
        [
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Kaguya",
                LastName = "Shinomiya",
                JapaneseName = "かぐや",
                JapaneseLastName = "四宮",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Main,
                Description =
                    "The student council vice president and heiress of the Shinomiya group, gifted but sheltered and proud."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Miyuki",
                LastName = "Shirogane",
                JapaneseName = "御行",
                JapaneseLastName = "白銀",
                Age = "17",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Main,
                Description =
                    "The hardworking student council president, who refuses to confess his love for Kaguya first."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Ai",
                LastName = "Hayasaka",
                JapaneseName = "愛",
                JapaneseLastName = "早坂",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "Kaguya's loyal personal attendant and confidante, who balances her duties with a fashion-loving teenage side."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Miko",
                LastName = "Iino",
                JapaneseName = "ミコ",
                JapaneseLastName = "伊井野",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "A small but fiercely serious student who runs against Shirogane for council president and strives to enforce the rules."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Kei",
                LastName = "Shirogane",
                JapaneseName = "圭",
                JapaneseLastName = "白銀",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "Miyuki's younger sister and treasurer of the middle school student council at Shuchiin Academy."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Chika",
                LastName = "Fujiwara",
                JapaneseName = "千花",
                JapaneseLastName = "藤原",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Main,
                Description =
                    "The scatterbrained, gossip-loving council secretary and Kaguya's friend since middle school, whose unpredictability keeps wrecking everyone's schemes."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Yuu",
                LastName = "Ishigami",
                JapaneseName = "優",
                JapaneseLastName = "石上",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Supporting,
                Description =
                    "The council's withdrawn treasurer, who prefers gaming at home to attending meetings and is shunned by classmates over a past incident."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Father Shirogane",
                JapaneseName = "白銀の父",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Supporting,
                Description =
                    "Miyuki and Kei's jobless dad, left behind by his wife after the family business collapsed, who happily cheers on Miyuki and Kaguya as a couple."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Maki",
                LastName = "Shijou",
                JapaneseName = "眞妃",
                JapaneseLastName = "四条",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "A second-year student from a legitimate branch of the Shinomiya bloodline, Kaguya's second cousin and born on the very same day."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Moeha",
                LastName = "Fujiwara",
                JapaneseName = "萌葉",
                JapaneseLastName = "藤原",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "Chika's younger sister and vice president of the middle school council, easily recognized by her twin ponytails and ribbons."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Nao",
                LastName = "Hayasaka",
                JapaneseName = "奈央",
                JapaneseLastName = "早坂",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description = "Ai's mother, who like her daughter is employed by the Shinomiya household."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Nagisa",
                LastName = "Kashiwagi",
                JapaneseName = "渚",
                JapaneseLastName = "柏木",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "A second-year and shipbuilding magnate's daughter who now and then turns to Kaguya for relationship advice."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Tsubame",
                LastName = "Koyasu",
                JapaneseName = "つばめ",
                JapaneseLastName = "子安",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "A third-year rhythmic gymnast and cheer squad vice-captain, counted among the school's four Impossible Girls, who gently draws Ishigami out of his shell."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Kobachi",
                LastName = "Osaragi",
                JapaneseName = "こばち",
                JapaneseLastName = "大仏",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "A bespectacled first-year, one of the Impossible Girls and a close friend of Miko."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Adolphe",
                LastName = "Pescarolo",
                JapaneseName = "アドルフ",
                JapaneseLastName = "ペスカロロ",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Supporting,
                Description =
                    "The sweet-toothed principal of Shuchiin Academy, who once also headed its sister school in Paris."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Tsubasa",
                LastName = "Tanuma",
                JapaneseName = "翼",
                JapaneseLastName = "田沼",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Supporting,
                Description =
                    "Nagisa's boyfriend, a second-year who sometimes seeks the president's guidance on love troubles."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Frieren",
                JapaneseName = "フリーレン",
                Age = "1000+",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Main,
                Description =
                    "A long-lived elf mage who defeated the Demon King and now travels to understand humans and her fallen companions."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Fern",
                JapaneseName = "フェルン",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Main,
                Description =
                    "Heiter's adopted daughter and Frieren's apprentice, serious and mature, who looks after the party like a mother."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Stark",
                JapaneseName = "シュタルク",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Main,
                Description = "A carefree and cowardly young warrior who overcomes his fear to protect those he loves."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Himmel",
                JapaneseName = "ヒンメル",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Supporting,
                Description =
                    "The vain but generous hero who led the party that defeated the Demon King and left a lasting mark on Frieren."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Eisen",
                JapaneseName = "アイゼン",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Supporting,
                Description = "A stoic and sturdy dwarf warrior who served as the hero party's frontline defender."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Heiter",
                JapaneseName = "ハイター",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Supporting,
                Description = "A kind and easygoing priest of the hero party who enjoys a drink despite his holy vows."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Übel",
                JapaneseName = "ユーベル",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "A smiling mage of the Continental Magic Association who cuts through anything she can picture, casting by feeling instead of theory."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Flamme",
                JapaneseName = "フランメ",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "Frieren's long-dead master, regarded as the greatest human mage ever and the woman who turned magic from a feared art into a human craft."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Sein",
                JapaneseName = "ザイン",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Supporting,
                Description =
                    "A laid-back village priest who briefly joins Frieren's group and acts as a grown-up voice for Fern and Stark."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Denken",
                JapaneseName = "デンケン",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Supporting,
                Description =
                    "An aged Imperial mage with a monocle and a long beard, calm and careful, who weighs every situation before a fight and values teamwork."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Aura",
                JapaneseName = "アウラ",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "A vain, articulate demon among the Seven Sages of Destruction, known as the Guillotine for beheading every foe she defeats."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Draht",
                JapaneseName = "ドラート",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Supporting,
                Description = "A young demon who serves as Lügner's assistant."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Edel",
                JapaneseName = "エーデル",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description = "A second-class mage with remarkable talent for mental magic."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Genau",
                JapaneseName = "ゲナウ",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Supporting,
                Description =
                    "The examiner overseeing the first-class mage exam, hardened by past losses and fighting with razor-sharp black wings."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Kanne",
                JapaneseName = "カンネ",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "A third-class mage and Lawine's friend since their academy days, who can shape and command water by filling it with mana."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Kraft",
                JapaneseName = "クラフト",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Supporting,
                Description =
                    "A cheerful, good-natured elf monk with many deeds to his name, who shared a long Northern Lands winter with Frieren's party."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Laufen",
                JapaneseName = "ラオフェン",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "A second-class mage on Denken's exam team, whose Southern Lands tribal magic lets her move too fast to follow."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Lawine",
                JapaneseName = "ラヴィーネ",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "A third-class mage and academy friend of Kanne, able to freeze enormous volumes of water."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Lügner",
                JapaneseName = "リュグナー",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Supporting,
                Description =
                    "A demon in Aura's service who comes across as polite, composed and gentle."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Scharf",
                JapaneseName = "シャルフ",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Supporting,
                Description =
                    "A third-class mage who befriends Wirbel and Ehre during the exam, using his magic to turn flower petals into steel constructs."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Qual",
                JapaneseName = "クヴァール",
                Role = CharacterRole.Supporting,
                Description =
                    "A senior demon of the Demon King's army, sealed away by the hero's party for creating Soul Track, a magic that pierces nearly any defense."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Serie",
                JapaneseName = "ゼーリエ",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "An ancient elf mage, Flamme's teacher and founder of the Continental Magic Association, whose mana and experience dwarf almost every living mage."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Sense",
                JapaneseName = "ゼンゼ",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "A pacifist examiner who designs a second exam without duels, and fights by controlling the length and strength of her hair."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Wirt",
                JapaneseName = "ヴィルト",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Supporting,
                Description =
                    "The eldest son of Lord Orden, who fell in battle against the demon army."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Rintarou",
                LastName = "Okabe",
                JapaneseName = "倫太郎",
                JapaneseLastName = "岡部",
                Age = "18",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Main,
                Description =
                    "A theatrical, self-proclaimed mad scientist and founder of the Future Gadget Lab."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Kurisu",
                LastName = "Makise",
                JapaneseName = "紅莉栖",
                JapaneseLastName = "牧瀬",
                Age = "18",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Main,
                Description = "A brilliant young neuroscience researcher, curious and proud, with a tsundere streak."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Mayuri",
                LastName = "Shiina",
                JapaneseName = "まゆり",
                JapaneseLastName = "椎名",
                Age = "16",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Main,
                Description = "Okabe's cheerful, optimistic childhood friend, who always seems slow and dreamy."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Suzuha",
                LastName = "Amane",
                JapaneseName = "鈴羽",
                JapaneseLastName = "阿万音",
                Age = "18",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "An energetic, bicycle-loving part-timer at the CRT TV Workshop who becomes a close ally of the Future Gadget Lab."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Itaru",
                LastName = "Hashida",
                JapaneseName = "至",
                JapaneseLastName = "橋田",
                Age = "19",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Main,
                Description =
                    "A talkative otaku and gifted hacker, nicknamed Daru, who is Okabe's loyal friend and the lab's programming expert."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Ruka",
                LastName = "Urushibara",
                JapaneseName = "るか",
                JapaneseLastName = "漆原",
                Age = "16",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Supporting,
                Description =
                    "A shy, polite and gentle classmate of Mayuri, so graceful that many take him for a girl, and who believes whatever Okabe tells him."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Rumiho",
                LastName = "Akiha",
                JapaneseName = "留未穂",
                JapaneseLastName = "秋葉",
                Age = "17",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "A childlike maid cafe waitress who punctuates her speech with cat-like meows and can read people's hearts by looking into their eyes."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Moeka",
                LastName = "Kiryuu",
                JapaneseName = "萌郁",
                JapaneseLastName = "桐生",
                Age = "20",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "A part-time editor who is almost never without her phone and prefers texting even in person, cheerful in messages but reserved face to face."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Yuugo",
                LastName = "Tennouji",
                JapaneseName = "裕吾",
                JapaneseLastName = "天王寺",
                Age = "32",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Supporting,
                Description =
                    "A widowed, bald and bearded landlord who runs the CRT TV Workshop below the lab and openly despises LCD screens."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Nae",
                LastName = "Tennouji",
                JapaneseName = "綯",
                JapaneseLastName = "天王寺",
                Age = "11",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "Tennouji's gentle young daughter, who visits the workshop after school, looks up to Mayuri as a big sister and is wary of Itaru and Okabe."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Nakabachi",
                JapaneseName = "中鉢",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Supporting,
                Description = "The professor who delivers the time travel lecture in the first episode."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Maho",
                LastName = "Hiyajou",
                JapaneseName = "真帆",
                JapaneseLastName = "比屋定",
                Age = "21",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "A fellow neuroscience researcher in Kurisu's lab at Viktor Chondria University, who attends a lecture at Okabe's university as Professor Leskinen's assistant."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Katsumi",
                LastName = "Nakase",
                JapaneseName = "克美",
                JapaneseLastName = "中瀬",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "A boyish-looking cosplayer known to her friends as Fubuki."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Judy",
                LastName = "Reyes",
                JapaneseName = "ジュディ",
                JapaneseLastName = "レイエス",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "A psychophysiology professor at Viktor Chondria University."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Kagari",
                LastName = "Shiina",
                JapaneseName = "かがり",
                JapaneseLastName = "椎名",
                Age = "10",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "A war orphan adopted by Mayuri in 2036, named after a bonfire in the hope that she would be a guiding light in a dark world."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Yuki",
                LastName = "Amane",
                JapaneseName = "由季",
                JapaneseLastName = "阿万音",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "A woman Okabe meets who looks uncannily like someone he knows."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Alexis",
                LastName = "Leskinen",
                JapaneseName = "アレクシス",
                JapaneseLastName = "レスキネン",
                Age = "34",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Supporting,
                Description =
                    "A respected American neuroscientist and mentor to Maho and Kurisu, boisterous and kind-hearted, with a childlike streak his assistant likes to point out."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Edward",
                LastName = "Elric",
                JapaneseName = "エドワード",
                JapaneseLastName = "エルリック",
                Age = "15-16",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Main,
                Description =
                    "A prodigy and the youngest State Alchemist, determined to restore his brother's body after a failed transmutation."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Alphonse",
                LastName = "Elric",
                JapaneseName = "アルフォンス",
                JapaneseLastName = "エルリック",
                Age = "14-15",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Main,
                Description = "Edward's younger brother, whose soul is bound to a suit of armor. Kind and gentle."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Roy",
                LastName = "Mustang",
                JapaneseName = "ロイ",
                JapaneseLastName = "マスタング",
                Age = "30",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Supporting,
                Description =
                    "The cunning Flame Alchemist and Edward's superior, who aims to reach the top of the military to reform the country."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Maes",
                LastName = "Hughes",
                JapaneseName = "マース",
                JapaneseLastName = "ヒューズ",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Supporting,
                Description =
                    "A sharp military investigator and Mustang's best friend, endlessly proud of his wife and daughter."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Winry",
                LastName = "Rockbell",
                JapaneseName = "ウィンリィ",
                JapaneseLastName = "ロックベル",
                Age = "15-16",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "A talented automail mechanic and childhood friend of the Elric brothers, who keeps Edward's prosthetics running."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Greed",
                JapaneseName = "グリード",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Supporting,
                Description =
                    "A homunculus who lives for luxury and power, able to harden his skin to a diamond-like shell, yet surprisingly loyal to his followers."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Riza",
                LastName = "Hawkeye",
                JapaneseName = "リザ",
                JapaneseLastName = "ホークアイ",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "Mustang's trusted aide and a deadly sharpshooter, who keeps him focused and in line while strictly training her dog, Black Hayate."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Ling",
                LastName = "Yao",
                JapaneseName = "リン",
                JapaneseLastName = "ヤオ",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Supporting,
                Description =
                    "A prince of Xing who travels to Amestris seeking the Philosopher's Stone to secure his clan's future, easygoing and often hungry but a capable fighter."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Alex Louis",
                LastName = "Armstrong",
                JapaneseName = "アレックス・ルイ",
                JapaneseLastName = "アームストロング",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Supporting,
                Description =
                    "A towering, muscular State Alchemist from a long line of alchemists, who fights with his fists and is fiercely protective of others."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Olivier Mira",
                LastName = "Armstrong",
                JapaneseName = "オリヴィエ・ミラ",
                JapaneseLastName = "アームストロング",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "Alex's older sister and the ruthless general guarding the Briggs northern border, who believes only the strong survive."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "King",
                LastName = "Bradley",
                JapaneseName = "キング",
                JapaneseLastName = "ブラッドレイ",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Supporting,
                Description =
                    "The eyepatch-wearing Führer of Amestris, who appears pleasant and good-humored while commanding the entire State military."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Selim",
                LastName = "Bradley",
                JapaneseName = "セリム",
                JapaneseLastName = "ブラッドレイ",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Supporting,
                Description =
                    "The Führer's young son, who admires Edward and dreams of becoming a State Alchemist to be of use to his father."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Mei",
                LastName = "Chang",
                JapaneseName = "メイ",
                JapaneseLastName = "チャン",
                Age = "11",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "A young Xingese princess who journeys to Amestris with her panda Xiao-Mei to find immortality for her clan, using alkahestry to fight from a distance."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Izumi",
                LastName = "Curtis",
                JapaneseName = "イズミ",
                JapaneseLastName = "カーティス",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "The strict, martial-arts-minded teacher of the Elric brothers, who sees them as her own sons and carries the scars of a failed human transmutation."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Trisha",
                LastName = "Elric",
                JapaneseName = "トリシャ",
                JapaneseLastName = "エルリック",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "The kind mother of Edward and Alphonse, whose early death from illness drives her sons to attempt the forbidden transmutation."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Envy",
                JapaneseName = "エンヴィー",
                Role = CharacterRole.Supporting,
                Description =
                    "A shapeshifting homunculus with a vicious temper and deep contempt for humans, who enjoys humiliating and hurting them."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Van",
                LastName = "Hohenheim",
                JapaneseName = "ヴァン",
                JapaneseLastName = "ホーエンハイム",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Supporting,
                Description =
                    "A brilliant, long-lived alchemist and the Elric brothers' father, who abandoned his family and is resented by Edward for it."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Elicia",
                LastName = "Hughes",
                JapaneseName = "エリシア",
                JapaneseLastName = "ヒューズ",
                Age = "3",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "The cheerful toddler daughter of Maes and Gracia Hughes, endlessly doted on by her father."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Solf J.",
                LastName = "Kimblee",
                JapaneseName = "ゾルフ・J",
                JapaneseLastName = "キンブリー",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Supporting,
                Description =
                    "A polite, unsettling Ishval War veteran and former State Alchemist who turns people into explosives and revels in destruction."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Lust",
                JapaneseName = "ラスト",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "A cunning, seductive homunculus whose sharp fingers can pierce almost anything, and who considers humans fragile and disposable."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Denji",
                JapaneseName = "デンジ",
                Age = "16",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Main,
                Description =
                    "A young man in debt who merges with the devil Pochita and becomes a devil hunter."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Power",
                JapaneseName = "パワー",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Main,
                Description =
                    "A blood fiend and Public Safety Devil Hunter, rude and dishonest but devoted to her cat Nyako."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Makima",
                JapaneseName = "マキマ",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Main,
                Description = "A Public Safety Devil Hunter who leads Denji and Power's squad."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Aki",
                LastName = "Hayakawa",
                JapaneseName = "アキ",
                JapaneseLastName = "早川",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Supporting,
                Description =
                    "A disciplined Public Safety Devil Hunter in Makima's squad, who mentors Denji and Power."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Reze",
                JapaneseName = "レゼ",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "A warm, friendly café worker who takes an instant liking to Denji, though her sweetness hides a darker side."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Kobeni",
                LastName = "Higashiyama",
                JapaneseName = "コベニ",
                JapaneseLastName = "東山",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "A timid, anxious new recruit of the Public Safety Devil Hunters who is constantly overwhelmed by danger."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Pochita",
                JapaneseName = "ポチタ",
                Role = CharacterRole.Supporting,
                Description =
                    "The small, chainsaw-headed devil who is Denji's closest companion and later becomes his heart."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Himeno",
                JapaneseName = "姫野",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "A Public Safety Devil Hunter in Makima's squad and Aki's partner, easygoing on the surface."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Kishibe",
                JapaneseName = "岸辺",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Supporting,
                Description =
                    "A scarred, grizzled captain of Tokyo Special Division 1 who wears piercings and a long coat over his Public Safety uniform."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Akane",
                LastName = "Sawatari",
                JapaneseName = "アカネ",
                JapaneseLastName = "沢渡",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "A former civilian devil hunter who works with the Gun Devil and holds a contract with a Snake Devil."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Samurai Sword",
                JapaneseName = "サムライソード",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Supporting,
                Description =
                    "A human-devil hybrid fused with the Katana Devil, who joins the Gun Devil's side to avenge his grandfather's death at Denji's hands."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Yotsuba",
                LastName = "Koiwai",
                JapaneseName = "四葉",
                JapaneseLastName = "小岩井",
                Age = "5",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Main,
                Description =
                    "A green-haired, endlessly curious little girl who turns every ordinary thing, from doorbells to cicadas, into a grand adventure."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Yousuke",
                LastName = "Koiwai",
                JapaneseName = "葉介",
                JapaneseLastName = "小岩井",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Main,
                Description =
                    "Yotsuba's easygoing adoptive father, a work-from-home translator who is a bit lazy but sincerely tries to be a good parent."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Fuuka",
                LastName = "Ayase",
                JapaneseName = "風香",
                JapaneseLastName = "綾瀬",
                Age = "16",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Main,
                Description =
                    "The responsible middle Ayase sister, often flustered by her family's quirks and fond of terrible puns and odd graphic shirts."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Asagi",
                LastName = "Ayase",
                JapaneseName = "あさぎ",
                JapaneseLastName = "綾瀬",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "The eldest Ayase sister, a university student with a teasing streak who is the best at handling Yotsuba."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Takashi",
                LastName = "Takeda",
                JapaneseName = "隆",
                JapaneseLastName = "竹田",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Supporting,
                Description =
                    "Known as Jumbo, a towering, deadpan florist and old classmate of Koiwai who is clumsy around women and treated like family by Yotsuba."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Musashi",
                LastName = "Miyamoto",
                JapaneseName = "武蔵",
                JapaneseLastName = "宮本",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Main,
                Description =
                    "Once a feared wild boy named Takezo, he wanders feudal Japan with a sword in hand, driven to become the strongest swordsman alive."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Kojirou",
                LastName = "Sasaki",
                JapaneseName = "小次郎",
                JapaneseLastName = "佐々木",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Main,
                Description =
                    "A deaf, mute and playful swordsman with uncanny reflexes, destined to become Musashi's greatest rival."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Matahachi",
                LastName = "Honiden",
                JapaneseName = "又八",
                JapaneseLastName = "本位田",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Main,
                Description =
                    "Musashi's pleasure-seeking childhood friend, who takes on another man's name and grows bitter and envious as their paths diverge."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Kaoruko",
                LastName = "Waguri",
                JapaneseName = "薫子",
                JapaneseLastName = "和栗",
                Age = "16",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Main,
                Description =
                    "A small, kind and sweet-loving scholarship student at Kikyo who sees past reputations and falls for a gentle boy from the rival school."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Rintarou",
                LastName = "Tsumugi",
                JapaneseName = "凛太郎",
                JapaneseLastName = "紬",
                Age = "16",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Main,
                Description =
                    "A tall, tired-looking student from Chidori who is mistaken for a delinquent but is kind-hearted and helps out at his family's bakery."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Subaru",
                LastName = "Hoshina",
                JapaneseName = "昴",
                JapaneseLastName = "保科",
                Age = "16",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "Kaoruko's silver-haired, protective childhood friend, wary of boys at first but won over once she sees their kindness."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Madoka",
                LastName = "Yuzuhara",
                JapaneseName = "まどか",
                JapaneseLastName = "柚原",
                Age = "17",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "A bespectacled, game-loving classmate of Kaoruko who is indifferent to the feud between the two schools."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Punpun",
                LastName = "Onodera",
                JapaneseName = "プンプン",
                JapaneseLastName = "小野寺",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Main,
                Description =
                    "An awkward boy drawn as a simple bird doodle, who stumbles through crushes, school and family troubles while searching for his place in the world."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Aiko",
                LastName = "Tanaka",
                JapaneseName = "愛子",
                JapaneseLastName = "田中",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Main,
                Description =
                    "An eccentric girl who becomes Punpun's first love and remains tied to his life for years."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Sachi",
                LastName = "Nanjou",
                JapaneseName = "幸",
                JapaneseLastName = "南条",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "An aspiring manga artist with blunt, realistic opinions who is often mistaken for rude and becomes an important friend to Punpun."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Masumi",
                LastName = "Seki",
                JapaneseName = "真澄",
                JapaneseLastName = "関",
                Gender = CharacterGender.Male,
                Role = CharacterRole.Supporting,
                Description =
                    "One of Punpun's childhood friends, a rough and cool-faced boy from a family that runs a lunch shop."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Chito",
                JapaneseName = "チト",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Main,
                Description =
                    "A cautious, bookish girl who travels the ruined world with Yuuri, always curious about its history and the people who came before."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Yuuri",
                JapaneseName = "ユーリ",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Main,
                Description =
                    "Chito's cheerful, resourceful and perpetually hungry companion, who keeps their spirits up in an empty, silent world."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Nuko",
                JapaneseName = "ヌコ",
                Role = CharacterRole.Supporting,
                Description =
                    "A strange pale creature that joins Chito and Yuuri on their trip and can change shape and communicate through radio signals."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Ishii",
                JapaneseName = "イシイ",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "A bespectacled engineer who repairs the girls' vehicle and asks for help building an aircraft in return."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Shijima",
                LastName = "Tsukishima",
                JapaneseName = "しじま",
                JapaneseLastName = "月島",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Main,
                Description =
                    "A pessimistic girl who spent two years shut in her room and now enters high school with shimeji mushrooms growing from her head."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Majime",
                LastName = "Yamashita",
                JapaneseName = "まじめ",
                JapaneseLastName = "山下",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Main,
                Description =
                    "Shijima's cheerful, carefree classmate, as unserious as her name suggests, with a sunny-side-up egg on her head."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Shijima's Older Sister",
                JapaneseName = "しじまの姉",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description =
                    "A university dropout who looks after Shijima like a parent while pursuing her own research into how reality works."
            },
            new Character
            {
                Id = Guid.NewGuid(),
                Name = "Mogawa-sensei",
                JapaneseName = "もがわ先生",
                Gender = CharacterGender.Female,
                Role = CharacterRole.Supporting,
                Description = "The teacher who advises the hole digging club."
            }
        ];
    }
}
