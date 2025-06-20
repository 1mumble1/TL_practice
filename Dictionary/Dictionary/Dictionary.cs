namespace Dictionary;

public class Dictionary
{
    private const char Delimeter = ':';
    private Dictionary<string, string> _dictionary = [];

    public void Initialize( string path )
    {
        using StreamReader reader = new( path );
        string? line;
        while ( ( line = reader.ReadLine() ) is not null )
        {
            string[] splittedLine = line.Split( Delimeter );
            if ( splittedLine.Length > 2 )
            {
                Console.WriteLine( $"Failed to recognize this line: {line}" );
                continue;
            }

            string word = splittedLine[ 0 ];
            string translations = splittedLine[ 1 ];

            if ( _dictionary.ContainsKey( word ) )
            {
                Console.WriteLine( $"Failed to add: {line}, because '{word}' already added" );
                continue;
            }
            _dictionary.Add( word, translations );
        }

        Console.ReadKey();
    }

    public bool IsEmpty()
    {
        return _dictionary.Count == 0;
    }

    public List<string>? FindWord( string word )
    {
        if ( _dictionary.ContainsKey( word ) )
        {
            string translate = _dictionary[ word ];
            return [ translate ];
        }

        if ( _dictionary.ContainsValue( word ) )
        {
            List<string> translates = _dictionary
                                        .Where( w => w.Value == word )
                                        .Select( w => w.Key )
                                        .ToList();
            return translates;
        }

        return null;
    }

    public bool AddNewWord( string word, string translation )
    {
        if ( _dictionary.ContainsKey( word ) )
        {
            return false;
        }

        _dictionary.Add( word, translation );
        return true;
    }

    public void SaveToFile( string path )
    {
        using StreamWriter sw = new StreamWriter( path );
        foreach ( var pair in _dictionary )
        {
            sw.WriteLine( $"{pair.Key}{Delimeter}{pair.Value}" );
        }
    }
}
