using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Runtime card database generated from Nahkor lore.
/// Creates all cards programmatically for use before ScriptableObjects are set up in the editor.
/// </summary>
[CreateAssetMenu(fileName = "CardDatabase", menuName = "MedievalCards/CardDatabase")]
public class CardDatabase : ScriptableObject
{
    public List<CardData> allCards = new();

    // ─── Factions ──────────────────────────────────────────────────────────────
    // Nahkor Bearers  → CardFaction.Kingdom
    // Matriarchal Empire → CardFaction.Empire
    // Zhraig          → CardFaction.Undead
    // Egoistikós      → CardFaction.Elves  (chaotic/ambitious)
    // Neutral / Cosmic → CardFaction.Neutral

    public static readonly CardDefinition[] Definitions = new CardDefinition[]
    {
        // ══════════════════════════════════════════════════════════════════════
        // PORTADORES NAHKOR  (Kingdom)
        // ══════════════════════════════════════════════════════════════════════
        new CardDefinition {
            cardName    = "Fritz, Portador del Vacío",
            description = "Forjado en la Batalla de los Falsos Emperadores, Fritz encontró una Nahkor cuyo filo adopta el negro más profundo. Su presencia inspira a sus aliados mientras espía las debilidades enemigas.",
            faction     = CardFaction.Kingdom,
            type        = CardType.Unit,
            row         = CardRow.Melee,
            basePower   = 7,
            isHero      = true,
            hasSpy      = true,   // Se juega en la fila enemiga → el dueño roba 2 cartas
        },
        new CardDefinition {
            cardName    = "Shirin, Compañera de Armas",
            description = "Compañera inseparable de Fritz, Shirin combina destreza marcial con una conexión a los Catalizadores que le permite curar a sus aliados caídos.",
            faction     = CardFaction.Kingdom,
            type        = CardType.Unit,
            row         = CardRow.Ranged,
            basePower   = 6,
            isHero      = true,
            hasMedic    = true,  // Revive una unidad del descarte
        },
        new CardDefinition {
            cardName    = "Itrich el Errante",
            description = "Vagabundo con un hábito ocre desgarrado y una espada oxidada, Itrich sobrevivió a la Gran Inundación. Su presencia eleva la moral de los más humildes.",
            faction     = CardFaction.Kingdom,
            type        = CardType.Unit,
            row         = CardRow.Melee,
            basePower   = 5,
            hasMorale   = true,  // +1 a todos los aliados de su fila
        },
        new CardDefinition {
            cardName    = "Aurora, Señora del Noroeste",
            description = "Gobernante de los latifundios fronterizos y los puertos mercantiles del oeste de Afthonia. Bajo su mando los ejércitos se duplican en fervor.",
            faction     = CardFaction.Kingdom,
            type        = CardType.Unit,
            row         = CardRow.Ranged,
            basePower   = 5,
            hasCommanderHorn = true, // Dobla la fuerza de todos los aliados de su fila
        },
        new CardDefinition {
            cardName    = "Clio, Aliada de Frontera",
            description = "Proveniente de una familia que llegó al poder junto a la gran dinastía, Clio es una estratega que multiplica el poder de sus iguales en el campo.",
            faction     = CardFaction.Kingdom,
            type        = CardType.Unit,
            row         = CardRow.Ranged,
            basePower   = 4,
            hasTightBond = true,  // Se duplica si hay otra Clio en la fila
        },
        new CardDefinition {
            cardName    = "Portador de la Oscuridad",
            description = "Uno de los 256 portadores de espadas Nahkor, elegidos por su maestría. No sobreviven más de un año: la matriarca se asegura de ello.",
            faction     = CardFaction.Kingdom,
            type        = CardType.Unit,
            row         = CardRow.Melee,
            basePower   = 4,
            hasMuster   = true,  // Invoca todas las copias del mazo
        },
        new CardDefinition {
            cardName    = "Jinete de Dragón Imperial",
            description = "Los hijos varones que nacen antes de la heredera son despojados de todo título y convertidos en jinetes de dragón. Letales en combate, invisibles en política.",
            faction     = CardFaction.Kingdom,
            type        = CardType.Unit,
            row         = CardRow.Siege,
            basePower   = 6,
        },
        new CardDefinition {
            cardName    = "La Quinta Hermana",
            description = "Personificación de sus cuatro hermanas Nahkor. Considerada una entidad más que una persona. Su muerte marcó el inicio de las guerras familiares.",
            faction     = CardFaction.Kingdom,
            type        = CardType.Unit,
            row         = CardRow.Any,
            basePower   = 10,
            isHero      = true,
            hasMorale   = true,
        },
        new CardDefinition {
            cardName    = "Guardiana de los Puertos de Merxias",
            description = "Las islas de Merxias dan vida a una federación mercante con una guardia costera temible. Donde hay comercio, hay espadas dispuestas a defenderlo.",
            faction     = CardFaction.Kingdom,
            type        = CardType.Unit,
            row         = CardRow.Ranged,
            basePower   = 3,
            hasTightBond = true,
        },

        // ══════════════════════════════════════════════════════════════════════
        // IMPERIO MATRIARCAL  (Empire)
        // ══════════════════════════════════════════════════════════════════════
        new CardDefinition {
            cardName    = "La Reina, Portadora de la Nada",
            description = "Heredera de la espada de la nada, lidera el Imperio con mano de hierro. Su espada Nahkor no tiene color: absorbe toda la luz a su alrededor.",
            faction     = CardFaction.Empire,
            type        = CardType.Leader,
            row         = CardRow.Any,
            basePower   = 12,
            isHero      = true,
            hasScorch   = true,  // Destruye la unidad con mayor fuerza en el campo
        },
        new CardDefinition {
            cardName    = "Matrona de Familia Servidora",
            description = "Las cinco familias servidoras proveen varones al linaje imperial y estabilizan la sucesión mediante enlaces matrimoniales. La matrona es quien teje esas alianzas.",
            faction     = CardFaction.Empire,
            type        = CardType.Unit,
            row         = CardRow.Ranged,
            basePower   = 4,
            hasMorale   = true,
        },
        new CardDefinition {
            cardName    = "Heredera Imperial",
            description = "Nacida tras múltiples intentos, la heredera es el eje del poder. Si muere antes de reinar, el Imperio entra en crisis. Inmune a las cartas de clima.",
            faction     = CardFaction.Empire,
            type        = CardType.Unit,
            row         = CardRow.Any,
            basePower   = 8,
            isHero      = true,
            hasMedic    = true,
        },
        new CardDefinition {
            cardName    = "Legionario del Gran Dominio",
            description = "Los soldados que unificaron el mundo conocido bajo una sola bandera. Disciplinados, abundantes y extremadamente leales.",
            faction     = CardFaction.Empire,
            type        = CardType.Unit,
            row         = CardRow.Melee,
            basePower   = 3,
            hasMuster   = true,
        },
        new CardDefinition {
            cardName    = "Dragón del Imperio",
            description = "Herramienta militar y símbolo del poder imperial. Los dragones son montados por jinetes desprovistos de título, pero no de fuerza.",
            faction     = CardFaction.Empire,
            type        = CardType.Unit,
            row         = CardRow.Siege,
            basePower   = 8,
            isHero      = false,
        },

        // ══════════════════════════════════════════════════════════════════════
        // ZHRAIG  (Undead)
        // ══════════════════════════════════════════════════════════════════════
        new CardDefinition {
            cardName    = "Señor Zhraig",
            description = "Los Zhraig son un problema que ningún reino quiso enfrentar a tiempo. Su señor porta una Nahkor cuyo filo es carmesí oscuro: el color de la rendición.",
            faction     = CardFaction.Undead,
            type        = CardType.Unit,
            row         = CardRow.Melee,
            basePower   = 7,
            isHero      = true,
            hasScorch   = true,
        },
        new CardDefinition {
            cardName    = "Guerrero Zhraig",
            description = "Salvaje y sin piedad. Los Zhraig no negocian; avanzan.",
            faction     = CardFaction.Undead,
            type        = CardType.Unit,
            row         = CardRow.Melee,
            basePower   = 4,
            hasTightBond = true,
        },
        new CardDefinition {
            cardName    = "Espectro de la Nahkor",
            description = "Cuando una espada Nahkor consume suficientes almas, el eco de sus víctimas puede materializarse en el campo de batalla.",
            faction     = CardFaction.Undead,
            type        = CardType.Unit,
            row         = CardRow.Any,
            basePower   = 3,
            hasMuster   = true,
        },

        // ══════════════════════════════════════════════════════════════════════
        // ERA EGOISTIKÓS  (Elves → facción caótica)
        // ══════════════════════════════════════════════════════════════════════
        new CardDefinition {
            cardName    = "Héroe Egoistikó",
            description = "En la era del caos que precedió al Gran Dominio, surgieron héroes movidos por la ambición más que por el honor. Las canciones los recuerdan; la historia los condena.",
            faction     = CardFaction.Elves,
            type        = CardType.Unit,
            row         = CardRow.Melee,
            basePower   = 6,
            hasSpy      = true,
        },
        new CardDefinition {
            cardName    = "Mercader de Rha'miras",
            description = "La federación de ciudades marinas de Rha'miras prospera gracias a sus mercaderes. Donde llegan ellos, llega información… y traición.",
            faction     = CardFaction.Elves,
            type        = CardType.Unit,
            row         = CardRow.Ranged,
            basePower   = 3,
            hasDecoy    = true,  // Intercambia con una unidad en el campo, vuelve a la mano
        },
        new CardDefinition {
            cardName    = "Senador de la Federación",
            description = "El gobierno de Rha'miras es una red de senadores que se vigilan mutuamente. Cada uno tiene sus propios intereses, pero juntos son invulnerables.",
            faction     = CardFaction.Elves,
            type        = CardType.Unit,
            row         = CardRow.Ranged,
            basePower   = 4,
            hasMorale   = true,
        },

        // ══════════════════════════════════════════════════════════════════════
        // NEUTRAL / CÓSMICO
        // ══════════════════════════════════════════════════════════════════════
        new CardDefinition {
            cardName    = "Catalizador del Origen",
            description = "Artefacto mitológico que dio forma al mundo. Posee las cuatro fuerzas primigenias: acelerar el tiempo, parar el tiempo, conservación y condenación.",
            faction     = CardFaction.Neutral,
            type        = CardType.Special,
            row         = CardRow.Any,
            basePower   = 0,
            hasCommanderHorn = true, // Dobla una fila entera
        },
        new CardDefinition {
            cardName    = "Bendición de Kor",
            description = "El Astro Madre baña el campo con su luz blanca de tono azul celeste. Las unidades aliadas se recuperan de los efectos del clima.",
            faction     = CardFaction.Neutral,
            type        = CardType.Special,
            row         = CardRow.Any,
            basePower   = 0,
            hasMedic    = true,
        },
        new CardDefinition {
            cardName    = "La Gran Inundación",
            description = "Cada dos ciclos, astros y lunas se alinean. Las aguas suben hasta diez varas inundando ciudades costeras. Reduce todas las unidades no-héroe de melee a 1.",
            faction     = CardFaction.Neutral,
            type        = CardType.Weather,
            row         = CardRow.Melee,
            basePower   = 0,
        },
        new CardDefinition {
            cardName    = "La Nahkor (Días de Oscuridad)",
            description = "Durante los tres días de oscuridad el Astro Padre se interpone. Sin luz, las Nahkor brillan más que nunca. Reduce todas las unidades no-héroe de asedio a 1.",
            faction     = CardFaction.Neutral,
            type        = CardType.Weather,
            row         = CardRow.Siege,
            basePower   = 0,
        },
        new CardDefinition {
            cardName    = "Tormenta Diaria",
            description = "Excepto en los días de oscuridad, las tormentas descargan cada noche sobre el mundo. Reduce todas las unidades no-héroe de distancia a 1.",
            faction     = CardFaction.Neutral,
            type        = CardType.Weather,
            row         = CardRow.Ranged,
            basePower   = 0,
        },
        new CardDefinition {
            cardName    = "El Octaedro",
            description = "Objeto de veneración para algunas religiones que creen que las espadas Nahkor son sus fragmentos. Nadie sabe qué es realmente. Nadie que lo haya tocado ha hablado.",
            faction     = CardFaction.Neutral,
            type        = CardType.Special,
            row         = CardRow.Any,
            basePower   = 0,
            hasScorch   = true,
        },
        new CardDefinition {
            cardName    = "Pasaje del Teóntelos",
            description = "Un fragmento del libro sagrado que legitima el orden del mundo. El que lo lee en voz alta inspira devoción incuestionable en sus aliados.",
            faction     = CardFaction.Neutral,
            type        = CardType.Special,
            row         = CardRow.Any,
            basePower   = 0,
            hasMorale   = true,
        },
        new CardDefinition {
            cardName    = "Señuelo Imperial",
            description = "Una figura de paja vestida con los colores del Imperio. Los enemigos atacan al señuelo mientras el verdadero objetivo escapa.",
            faction     = CardFaction.Neutral,
            type        = CardType.Special,
            row         = CardRow.Any,
            basePower   = 0,
            hasDecoy    = true,
        },
    };
}

/// <summary>Plain data struct used to define cards before ScriptableObjects are created in the editor.</summary>
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
