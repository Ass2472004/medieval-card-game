using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Runtime card database — 50 cards from the world of Nahkor.
/// </summary>
[CreateAssetMenu(fileName = "CardDatabase", menuName = "MedievalCards/CardDatabase")]
public class CardDatabase : ScriptableObject
{
    public List<CardData> allCards = new();

    public static readonly CardDefinition[] Definitions = new CardDefinition[]
    {
        // ══════════════════════════════════════════════════════════════════════
        // PORTADORES NAHKOR  — Kingdom  (17 cartas)
        // ══════════════════════════════════════════════════════════════════════

        new CardDefinition {
            cardName    = "Fritz, Portador del Vacío",
            description = "Forjado en la Batalla de los Falsos Emperadores. Su Nahkor es de negro absoluto. Espía las debilidades enemigas mientras inspira a sus aliados.",
            faction     = CardFaction.Kingdom,
            type        = CardType.Unit,
            row         = CardRow.Melee,
            basePower   = 7,
            isHero      = true,
            hasSpy      = true,
        },
        new CardDefinition {
            cardName    = "Shirin, Compañera de Armas",
            description = "Compañera de Fritz. Su conexión con los Catalizadores le permite devolver la vida a los caídos en combate.",
            faction     = CardFaction.Kingdom,
            type        = CardType.Unit,
            row         = CardRow.Ranged,
            basePower   = 6,
            isHero      = true,
            hasMedic    = true,
        },
        new CardDefinition {
            cardName    = "Itrich el Errante",
            description = "Sobreviviente de la Gran Inundación. Su presencia, por humilde que sea, eleva la moral de los más olvidados.",
            faction     = CardFaction.Kingdom,
            type        = CardType.Unit,
            row         = CardRow.Melee,
            basePower   = 5,
            hasMorale   = true,
        },
        new CardDefinition {
            cardName    = "Aurora, Señora del Noroeste",
            description = "Gobernante de los latifundios fronterizos y los puertos mercantiles del oeste de Afthonia. Su presencia dobla el fervor de su fila.",
            faction     = CardFaction.Kingdom,
            type        = CardType.Unit,
            row         = CardRow.Ranged,
            basePower   = 5,
            isHero      = true,
            hasCommanderHorn = true,
        },
        new CardDefinition {
            cardName    = "Clio, Aliada de Frontera",
            description = "Estratega de la familia que llegó al poder junto a la gran dinastía. Se multiplica junto a sus iguales.",
            faction     = CardFaction.Kingdom,
            type        = CardType.Unit,
            row         = CardRow.Ranged,
            basePower   = 4,
            hasTightBond = true,
        },
        new CardDefinition {
            cardName    = "La Quinta Hermana",
            description = "Personificación de sus cuatro hermanas Nahkor. Su muerte inició las guerras familiares. Nadie sabe si realmente murió.",
            faction     = CardFaction.Kingdom,
            type        = CardType.Unit,
            row         = CardRow.Any,
            basePower   = 10,
            isHero      = true,
            hasMorale   = true,
        },
        new CardDefinition {
            cardName    = "Primera Hermana, Filo Carmesí",
            description = "La mayor de las hijas del gran héroe. Su Nahkor es carmesí. Controlaba la cuarta parte de los 256 portadores y no toleraba rivales.",
            faction     = CardFaction.Kingdom,
            type        = CardType.Unit,
            row         = CardRow.Melee,
            basePower   = 9,
            isHero      = true,
            hasScorch   = true,
        },
        new CardDefinition {
            cardName    = "Segunda Hermana, Filo Índigo",
            description = "Sanadora y estratega. Su Nahkor índigo devuelve a los caídos al campo de batalla, pero a un precio.",
            faction     = CardFaction.Kingdom,
            type        = CardType.Unit,
            row         = CardRow.Ranged,
            basePower   = 8,
            isHero      = true,
            hasMedic    = true,
        },
        new CardDefinition {
            cardName    = "Tercera Hermana, Filo Esmeralda",
            description = "La más astuta. Su espada esmeralda la hace invisible a los ojos enemigos. Infiltra sus unidades en el campo contrario.",
            faction     = CardFaction.Kingdom,
            type        = CardType.Unit,
            row         = CardRow.Ranged,
            basePower   = 8,
            isHero      = true,
            hasSpy      = true,
        },
        new CardDefinition {
            cardName    = "Cuarta Hermana, Filo Dorado",
            description = "La más joven de las cuatro mortales. Su fuerza se multiplica junto a sus hermanas. Inseparables en combate.",
            faction     = CardFaction.Kingdom,
            type        = CardType.Unit,
            row         = CardRow.Melee,
            basePower   = 8,
            isHero      = true,
            hasTightBond = true,
        },
        new CardDefinition {
            cardName    = "Portador de la Oscuridad",
            description = "Uno de los 256. Elegido por su maestría. No vive más de un año: la matriarca se encarga de ello. Hay muchos como él.",
            faction     = CardFaction.Kingdom,
            type        = CardType.Unit,
            row         = CardRow.Melee,
            basePower   = 4,
            hasMuster   = true,
        },
        new CardDefinition {
            cardName    = "Portador Veterano",
            description = "Ha sobrevivido dos semanas más de lo que debería. Eso lo convierte en una leyenda entre los suyos y en un peligro para la matriarca.",
            faction     = CardFaction.Kingdom,
            type        = CardType.Unit,
            row         = CardRow.Melee,
            basePower   = 5,
            hasMorale   = true,
        },
        new CardDefinition {
            cardName    = "Jinete de Dragón Imperial",
            description = "Hijo varón nacido antes que la heredera. Despojado de todo título, armado con todo el entrenamiento. Letal desde las alturas.",
            faction     = CardFaction.Kingdom,
            type        = CardType.Unit,
            row         = CardRow.Siege,
            basePower   = 6,
        },
        new CardDefinition {
            cardName    = "Explorador de Merxias",
            description = "Las islas de Merxias forman una federación mercante. Sus exploradores son expertos en infiltrarse en filas enemigas a cambio de información.",
            faction     = CardFaction.Kingdom,
            type        = CardType.Unit,
            row         = CardRow.Ranged,
            basePower   = 3,
            hasSpy      = true,
        },
        new CardDefinition {
            cardName    = "Guardiana de los Puertos",
            description = "Donde hay comercio en Merxias, hay espadas dispuestas a defenderlo. Juntas son infranqueables.",
            faction     = CardFaction.Kingdom,
            type        = CardType.Unit,
            row         = CardRow.Ranged,
            basePower   = 3,
            hasTightBond = true,
        },
        new CardDefinition {
            cardName    = "Sacerdote de Kor",
            description = "Devoto del Astro Madre. En una sociedad tan religiosa como la del norte de Afthonia, sus palabras mueven ejércitos.",
            faction     = CardFaction.Kingdom,
            type        = CardType.Unit,
            row         = CardRow.Ranged,
            basePower   = 3,
            hasMorale   = true,
        },
        new CardDefinition {
            cardName    = "Vidente del Teóntelos",
            description = "Interpreta los pasajes del libro sagrado. Donde los otros ven una batalla perdida, él ve el camino para traer de vuelta a los caídos.",
            faction     = CardFaction.Kingdom,
            type        = CardType.Unit,
            row         = CardRow.Ranged,
            basePower   = 2,
            hasMedic    = true,
        },

        // ══════════════════════════════════════════════════════════════════════
        // IMPERIO MATRIARCAL  — Empire  (9 cartas)
        // ══════════════════════════════════════════════════════════════════════

        new CardDefinition {
            cardName    = "La Reina, Portadora de la Nada",
            description = "Hereda la espada de la nada. Su filo no tiene color porque absorbe toda la luz. El Imperio tiembla cuando desenvaina.",
            faction     = CardFaction.Empire,
            type        = CardType.Leader,
            row         = CardRow.Any,
            basePower   = 12,
            isHero      = true,
            hasScorch   = true,
        },
        new CardDefinition {
            cardName    = "Heredera Imperial",
            description = "Eje de la sucesión. Si muere antes de reinar, el Imperio entra en caos. Inmune al clima. Capaz de restaurar a los caídos.",
            faction     = CardFaction.Empire,
            type        = CardType.Unit,
            row         = CardRow.Any,
            basePower   = 8,
            isHero      = true,
            hasMedic    = true,
        },
        new CardDefinition {
            cardName    = "Matrona de Familia Servidora",
            description = "Las cinco familias servidoras tejen alianzas matrimoniales y garantizan la estabilidad del linaje. La matrona es quien mueve los hilos.",
            faction     = CardFaction.Empire,
            type        = CardType.Unit,
            row         = CardRow.Ranged,
            basePower   = 4,
            hasMorale   = true,
        },
        new CardDefinition {
            cardName    = "Gran Consejera Imperial",
            description = "No lleva espada sino palabras. Intercambia posiciones en el campo con la precisión de quien ha jugado este juego toda su vida.",
            faction     = CardFaction.Empire,
            type        = CardType.Unit,
            row         = CardRow.Ranged,
            basePower   = 5,
            hasDecoy    = true,
        },
        new CardDefinition {
            cardName    = "Arquera de la Guardia Imperial",
            description = "Entrenada desde la infancia en los dominios del Imperio. Formidable en grupo.",
            faction     = CardFaction.Empire,
            type        = CardType.Unit,
            row         = CardRow.Ranged,
            basePower   = 4,
            hasTightBond = true,
        },
        new CardDefinition {
            cardName    = "Capitán de la Guardia",
            description = "Eleva a sus hombres con su sola presencia. Sin él, la guardia imperial es una fuerza mediocre; con él, son imparables.",
            faction     = CardFaction.Empire,
            type        = CardType.Unit,
            row         = CardRow.Melee,
            basePower   = 5,
            hasMorale   = true,
        },
        new CardDefinition {
            cardName    = "Legionario del Gran Dominio",
            description = "Los soldados que unificaron el mundo conocido. Disciplinados y abundantes. Donde cae uno, vienen tres más.",
            faction     = CardFaction.Empire,
            type        = CardType.Unit,
            row         = CardRow.Melee,
            basePower   = 3,
            hasMuster   = true,
        },
        new CardDefinition {
            cardName    = "Dragón del Imperio",
            description = "Herramienta militar y símbolo de poder. Montado por jinetes sin título pero llenos de furia.",
            faction     = CardFaction.Empire,
            type        = CardType.Unit,
            row         = CardRow.Siege,
            basePower   = 8,
        },
        new CardDefinition {
            cardName    = "Ballestero del Reino Septentrional",
            description = "Lo que queda de la vieja dinastía se arrinconan en el norte. Sus ballesteros recuerdan tiempos mejores y disparan con la rabia de quien lo ha perdido todo.",
            faction     = CardFaction.Empire,
            type        = CardType.Unit,
            row         = CardRow.Ranged,
            basePower   = 4,
            hasTightBond = true,
        },

        // ══════════════════════════════════════════════════════════════════════
        // ZHRAIG  — Undead  (7 cartas)
        // ══════════════════════════════════════════════════════════════════════

        new CardDefinition {
            cardName    = "Señor Zhraig",
            description = "Ningún reino quiso enfrentarlos a tiempo. El Señor Zhraig porta una Nahkor carmesí oscuro: el color de la rendición ajena.",
            faction     = CardFaction.Undead,
            type        = CardType.Unit,
            row         = CardRow.Melee,
            basePower   = 7,
            isHero      = true,
            hasScorch   = true,
        },
        new CardDefinition {
            cardName    = "Chamán Zhraig",
            description = "No entienden de médicos, entienden de rituales. El chamán devuelve a los caídos de maneras que es mejor no preguntar.",
            faction     = CardFaction.Undead,
            type        = CardType.Unit,
            row         = CardRow.Ranged,
            basePower   = 4,
            hasMedic    = true,
        },
        new CardDefinition {
            cardName    = "Explorador Zhraig",
            description = "Silencioso y eficaz. Se infiltra en filas enemigas antes de que nadie lo note. Cuando lo notan, ya es tarde.",
            faction     = CardFaction.Undead,
            type        = CardType.Unit,
            row         = CardRow.Ranged,
            basePower   = 3,
            hasSpy      = true,
        },
        new CardDefinition {
            cardName    = "Guerrero Zhraig",
            description = "Salvaje y sin piedad. Los Zhraig no negocian: avanzan. Más peligrosos en grupo.",
            faction     = CardFaction.Undead,
            type        = CardType.Unit,
            row         = CardRow.Melee,
            basePower   = 4,
            hasTightBond = true,
        },
        new CardDefinition {
            cardName    = "Bestia de los Zhraig",
            description = "Criatura sin nombre que los Zhraig arrojan al campo cuando todo lo demás falla. Destruye lo más fuerte que encuentre.",
            faction     = CardFaction.Undead,
            type        = CardType.Unit,
            row         = CardRow.Melee,
            basePower   = 7,
            hasScorch   = true,
        },
        new CardDefinition {
            cardName    = "Espectro de la Nahkor",
            description = "Cuando una espada consume suficientes almas, los ecos se materializan. Hay muchos. Demasiados.",
            faction     = CardFaction.Undead,
            type        = CardType.Unit,
            row         = CardRow.Any,
            basePower   = 3,
            hasMuster   = true,
        },
        new CardDefinition {
            cardName    = "Horda de la Oscuridad",
            description = "En los días de Nahkor los Zhraig atacan sin aviso. La oscuridad es su aliada más fiel.",
            faction     = CardFaction.Undead,
            type        = CardType.Unit,
            row         = CardRow.Melee,
            basePower   = 5,
            hasMorale   = true,
        },

        // ══════════════════════════════════════════════════════════════════════
        // ERA EGOISTIKÓS  — Elves  (7 cartas)
        // ══════════════════════════════════════════════════════════════════════

        new CardDefinition {
            cardName    = "Héroe Egoistikó",
            description = "Los héroes de la era del caos eran recordados en canciones y condenados en los libros. Movidos por ambición, no por honor.",
            faction     = CardFaction.Elves,
            type        = CardType.Unit,
            row         = CardRow.Melee,
            basePower   = 6,
            hasSpy      = true,
        },
        new CardDefinition {
            cardName    = "Ambicioso Egoistikó",
            description = "En la era del caos, la ambición era la única moneda. Espía, traiciona, avanza.",
            faction     = CardFaction.Elves,
            type        = CardType.Unit,
            row         = CardRow.Melee,
            basePower   = 5,
            hasSpy      = true,
        },
        new CardDefinition {
            cardName    = "Saqueador de la Era Caótica",
            description = "Cuando la religión se debilitó y los dioses aún no habían llegado, estos hombres llenaron el vacío con violencia.",
            faction     = CardFaction.Elves,
            type        = CardType.Unit,
            row         = CardRow.Melee,
            basePower   = 4,
            hasTightBond = true,
        },
        new CardDefinition {
            cardName    = "Profeta del Caos",
            description = "Antes de que los dioses llegaran a confirmar las profecías, este hombre ya las anunciaba. Nadie le creyó. Todos deberían haberlo hecho.",
            faction     = CardFaction.Elves,
            type        = CardType.Unit,
            row         = CardRow.Ranged,
            basePower   = 3,
            hasMorale   = true,
        },
        new CardDefinition {
            cardName    = "Mercader de Rha'miras",
            description = "La federación de ciudades marinas prospera con el comercio. Sus mercaderes llevan información en lugar de mercancía.",
            faction     = CardFaction.Elves,
            type        = CardType.Unit,
            row         = CardRow.Ranged,
            basePower   = 3,
            hasDecoy    = true,
        },
        new CardDefinition {
            cardName    = "Senador de la Federación",
            description = "El gobierno de Rha'miras es una red de senadores que se vigilan mutuamente. Juntos son invulnerables.",
            faction     = CardFaction.Elves,
            type        = CardType.Unit,
            row         = CardRow.Ranged,
            basePower   = 4,
            hasMorale   = true,
        },
        new CardDefinition {
            cardName    = "Navegante de Angkor",
            description = "Los mares entre Angkor y Na'vi son traicioneros. Los que los cruzan son más peligrosos aún.",
            faction     = CardFaction.Elves,
            type        = CardType.Unit,
            row         = CardRow.Siege,
            basePower   = 5,
            hasTightBond = true,
        },

        // ══════════════════════════════════════════════════════════════════════
        // NEUTRAL / CÓSMICO  (10 cartas)
        // ══════════════════════════════════════════════════════════════════════

        new CardDefinition {
            cardName    = "Catalizador de la Aceleración",
            description = "Una de las cuatro fuerzas primigenias. Acelera la percepción del portador: actúa antes que nadie en el campo.",
            faction     = CardFaction.Neutral,
            type        = CardType.Special,
            row         = CardRow.Any,
            basePower   = 0,
            hasCommanderHorn = true,
        },
        new CardDefinition {
            cardName    = "Catalizador de la Detención",
            description = "Para el tiempo en la mente del adversario. Sus pensamientos se vuelven lentos, sus decisiones, erróneas.",
            faction     = CardFaction.Neutral,
            type        = CardType.Special,
            row         = CardRow.Any,
            basePower   = 0,
            hasDecoy    = true,
        },
        new CardDefinition {
            cardName    = "Catalizador de la Conservación",
            description = "La fuerza que mantiene vivo todo lo que existe. Devuelve a los caídos. Ninguna herida es definitiva para quien lo posee.",
            faction     = CardFaction.Neutral,
            type        = CardType.Special,
            row         = CardRow.Any,
            basePower   = 0,
            hasMedic    = true,
        },
        new CardDefinition {
            cardName    = "Catalizador de la Condenación",
            description = "El fin de las cosas. Nadie sabe si hay algo después. La unidad con mayor fuerza en el campo descubre la respuesta.",
            faction     = CardFaction.Neutral,
            type        = CardType.Special,
            row         = CardRow.Any,
            basePower   = 0,
            hasScorch   = true,
        },
        new CardDefinition {
            cardName    = "La Gran Inundación",
            description = "Cada dos ciclos, astros y lunas se alinean. Las aguas suben diez varas. Todas las unidades no-héroe de melee bajan a 1.",
            faction     = CardFaction.Neutral,
            type        = CardType.Weather,
            row         = CardRow.Melee,
            basePower   = 0,
        },
        new CardDefinition {
            cardName    = "La Nahkor — Días de Oscuridad",
            description = "El Astro Padre se interpone. Sin luz, las espadas Nahkor brillan más que nunca. Unidades no-héroe de asedio bajan a 1.",
            faction     = CardFaction.Neutral,
            type        = CardType.Weather,
            row         = CardRow.Siege,
            basePower   = 0,
        },
        new CardDefinition {
            cardName    = "Tormenta Diaria",
            description = "Excepto en los días de oscuridad, las tormentas descargan cada noche. Unidades no-héroe de distancia bajan a 1.",
            faction     = CardFaction.Neutral,
            type        = CardType.Weather,
            row         = CardRow.Ranged,
            basePower   = 0,
        },
        new CardDefinition {
            cardName    = "El Octaedro",
            description = "Algunas religiones creen que las espadas Nahkor son sus fragmentos. Nadie sabe qué es. Nadie que lo haya tocado ha hablado.",
            faction     = CardFaction.Neutral,
            type        = CardType.Special,
            row         = CardRow.Any,
            basePower   = 0,
            hasScorch   = true,
        },
        new CardDefinition {
            cardName    = "Bendición de Kor",
            description = "El Astro Madre baña el campo con luz blanca de tono azul celeste. Elimina todos los efectos de clima activos.",
            faction     = CardFaction.Neutral,
            type        = CardType.Special,
            row         = CardRow.Any,
            basePower   = 0,
            hasMedic    = true,
        },
        new CardDefinition {
            cardName    = "Batalla de los Falsos Emperadores",
            description = "El evento que lo cambió todo. De ahí salió la dinastía que unificó el mundo conocido. Y de ahí salió Fritz con una espada que no debería haber encontrado.",
            faction     = CardFaction.Neutral,
            type        = CardType.Special,
            row         = CardRow.Any,
            basePower   = 0,
            hasCommanderHorn = true,
        },
    };
}

[System.Serializable]
public class CardDefinition
{
    public string      cardName;
    public string      description;
    public CardFaction faction;
    public CardType    type;
    public CardRow     row;
    public int         basePower;
    public bool        isHero;
    public bool        hasMuster;
    public bool        hasSpy;
    public bool        hasMedic;
    public bool        hasTightBond;
    public bool        hasMorale;
    public bool        hasScorch;
    public bool        hasDecoy;
    public bool        hasCommanderHorn;
}
