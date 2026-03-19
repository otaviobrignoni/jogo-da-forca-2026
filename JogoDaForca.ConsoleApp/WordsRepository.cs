namespace JogoDaForca.ConsoleApp;

using System.Security.Cryptography;

public class WordsRepository
{
    public record Category(string Name, string[] Words);
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    public static readonly Category[] EasyCategories =
    [
        new("Animais", [
            "CACHORRO","GATO","PATO","RATO","CAVALO","VACA","PORCO","OVELHA","CABRA","LEÃO","TIGRE",
            "ELEFANTE","GIRAFA","ZEBRA","URSO","LOBO","RAPOSA","MACACO","COELHO","TARTARUGA","JACARÉ",
            "COBRA","ÁGUIA","POMBO","GALINHA","PEIXE","BALEIA"
        ]),
        new("Cores", [
            "VERMELHO","AZUL","VERDE","AMARELO","ROXO","LARANJA","PRETO","BRANCO","CINZA","MARROM",
            "BEGE","ROSA","MAGENTA","CÍAN","DOURADO","PRATEADO","VINHO","TURQUESA","INDIGO","CORAL",
            "CREME","MOSTARDA","AZUL MARINHO","VERDE LIMÃO","BORDÔ"
        ]),
        new("Vegetais", [
            "CENOURA","BATATA","BATATA DOCE","MANDIOCA","INHAME","BETERRABA","ABÓBORA","ABOBRINHA",
            "BERINJELA","TOMATE","ALFACE","RÚCULA","ESPINAFRE","COUVE","REPOLHO","BRÓCOLIS",
            "COUVE FLOR","ERVILHA","MILHO","PIMENTÃO","PEPINO","CEBOLA","ALHO","ALHO PORÓ","NABO"
        ]),
        new("Verbos", [
            "COMER","BEBER","ANDAR","CORRER","PULAR","FALAR","OUVIR","VER","OLHAR","TOCAR","SENTIR",
            "DORMIR","ACORDAR","TRABALHAR","ESTUDAR","APRENDER","ENSINAR","ABRIR","FECHAR","ENTRAR",
            "SAIR","SUBIR","DESCER","PEGAR","LARGAR","ESCREVER","LER","PENSAR"
        ]),
    ];

    public static readonly Category[] MediumCategories =
    [
        new("Frutas", [
            "ABACAXI","ACEROLA","AÇAÍ","ARAÇÁ","ABACATE","BACABA","BACURI","BANANA","CAJÁ","CAJU",
            "CARAMBOLA","CUPUAÇU","GRAVIOLA","GOIABA","JABUTICABA","JENIPAPO","MAÇÃ","MANGABA",
            "MANGA","MARACUJÁ","MURICI","PEQUI","PITANGA","PITAYA","SAPOTI","TANGERINA","UMBU",
            "UVA","UVAIA","KIWI","LARANJA","LIMÃO","MELANCIA","MELÃO","PÊRA"
        ]),
        new("Corpo Humano", [
            "CABEÇA","OLHO","ORELHA","NARIZ","BOCA","DENTE","LÍNGUA","PESCOÇO","OMBRO","BRAÇO",
            "COTOVELO","MÃO","DEDO","PEITO","COSTAS","BARRIGA","UMBIGO","QUADRIL","PERNA","JOELHO",
            "TORNOZELO","PÉ","CALCANHAR","UNHA","PELE","CORAÇÃO","CÉREBRO"
        ]),
        new("Plantas", [
            "ÁRVORE","PLANTA","ARBUSTO","SAMAMBAIA","CACTO","PINHEIRO","EUCALIPTO","BAMBU","PALMEIRA",
            "CARVALHO","CEDRO","IPÊ","JACARANDÁ","SUCULENTA","HERA","MUSGO","TREPADEIRA","HORTELÃ",
            "MANJERICÃO","ALECRIM","SÁLVIA","COENTRO","CAPIM","FLOR","RAIZ","SEMENTE"
        ]),
        new("Eletrodomésticos", [
            "GELADEIRA","FOGÃO","MICROONDAS","LIQUIDIFICADOR","BATEDEIRA","CAFETEIRA","TORRADEIRA",
            "AIRFRYER","ASPIRADOR","VENTILADOR","AR CONDICIONADO","AQUECEDOR","FERRO","SECADOR",
            "CHAPINHA","FREEZER","FORNO","LAVA LOUÇAS","LAVA ROUPAS","SECADORA","COIFA","EXAUSTOR",
            "PURIFICADOR","UMIDIFICADOR","DESUMIDIFICADOR"
        ]),
        new("Flores", [
            "ROSA","TULIPA","ORQUÍDEA","MARGARIDA","GIRASSOL","LÍRIO","CRAVO","AZALEIA","HORTÊNSIA",
            "VIOLETA","JASMIM","CAMÉLIA","HIBISCO","PETÚNIA","BEGÔNIA","NARCISO","LAVANDA","ÍRIS",
            "DÁLIA","AMARÍLIS","PRÍMULA","ANGÉLICA","FLORES DO CAMPO","ALFAZEMA","CRISÂNTEMO"
        ]),
    ];

    public static readonly Category[] HardCategories =
    [
        new("Países", [
            "BRASIL","ARGENTINA","CHILE","PERU","COLÔMBIA","VENEZUELA","URUGUAI","PARAGUAI","BOLÍVIA",
            "MÉXICO","CANADÁ","ESTADOS UNIDOS","PORTUGAL","ESPANHA","FRANÇA","ALEMANHA","ITÁLIA",
            "HOLANDA","BÉLGICA","SUÍÇA","SUÉCIA","NORUEGA","DINAMARCA","POLÔNIA","UCRÂNIA","RÚSSIA",
            "CHINA","JAPÃO","COREIA DO SUL","ÍNDIA","AUSTRÁLIA","NOVA ZELÂNDIA"
        ]),
        new("Cidades", [
            "SÃO PAULO","RIO DE JANEIRO","CURITIBA","PORTO ALEGRE","FLORIANÓPOLIS","SALVADOR",
            "RECIFE","FORTALEZA","BRASÍLIA","BELO HORIZONTE","LISBOA","PORTO","MADRI","BARCELONA",
            "PARIS","LONDRES","ROMA","MILÃO","BERLIM","AMSTERDÃ","NOVA IORQUE","LOS ANGELES",
            "TÓQUIO","SEUL","PEQUIM","DUBAI","SIDNEY","TORONTO"
        ]),
        new("Objetos", [
            "MESA","CADEIRA","SOFÁ","CAMA","ARMÁRIO","PRATELEIRA","LIVRO","CADERNO","CANETA","LÁPIS",
            "BOLSA","MOCHILA","RELÓGIO","ÓCULOS","CHAVE","PORTA","JANELA","ESPELHO","TELEVISÃO",
            "CONTROLE","FONE","CARREGADOR","CABO","CAIXA","GARRAFA","COPO"
        ]),
        new("Marcas", [
            "NIKE","ADIDAS","PUMA","APPLE","SAMSUNG","SONY","LG","MICROSOFT","GOOGLE","AMAZON",
            "NETFLIX","SPOTIFY","COCA COLA","PEPSI","NESTLÉ","KFC","MCDONALDS","BURGER KING",
            "STARBUCKS","HBO","DISNEY","INTEL","AMD","NVIDIA","TESLA"
        ]),
        new("Montadoras", [
            "VOLKSWAGEN","FIAT","FORD","CHEVROLET","TOYOTA","HONDA","HYUNDAI","KIA","NISSAN",
            "RENAULT","PEUGEOT","CITROEN","BMW","MERCEDES","AUDI","PORSCHE","FERRARI","LAMBORGHINI",
            "TESLA","JEEP","RAM","DODGE","MAZDA","SUBARU","SUZUKI"
        ]),
    ];
    public static Category[] GetCategories(Difficulty difficulty)
    {
        return difficulty switch
        {
            Difficulty.Easy => EasyCategories,
            Difficulty.Medium => MediumCategories,
            Difficulty.Hard => HardCategories,
            _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
        };
    }
    public static string GetRandomWord(Category category)
    {
        return category.Words[RandomNumberGenerator.GetInt32(category.Words.Length)];
    }
}