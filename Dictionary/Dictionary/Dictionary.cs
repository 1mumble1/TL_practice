namespace Dictionary;

public class Dictionary
{
    private const char Delimeter = ':';
    private Dictionary<string, string> _dictionary = new();

    public void Initialize( string path )
    {
        using ( StreamReader reader = new StreamReader( path ) )
        {
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

            Console.WriteLine( "Press any button to continue work with dictionary..." );
            Console.ReadKey();
        }
    }

    public bool IsEmpty()
    {
        return _dictionary.Count == 0;
    }

    public bool FindWord( string word, out string translate )
    {
        if ( _dictionary.ContainsKey( word ) )
        {
            translate = _dictionary[ word ];
            return true;
        }

        if ( _dictionary.ContainsValue( word ) )
        {
            translate = _dictionary.FirstOrDefault( w => w.Value == word ).Key;
            return true;
        }

        translate = word;
        return false;
    }

    public bool AddNewWord( string word, string translation )
    {
        if ( !_dictionary.ContainsKey( word ) )
        {
            _dictionary.Add( word, translation );
            return true;
        }
        return false;
    }

    public void SaveToFile( string path )
    {
        using ( StreamWriter sw = new StreamWriter( path ) )
        {
            foreach ( var pair in _dictionary )
            {
                sw.WriteLine( $"{pair.Key}{Delimeter}{pair.Value}" );
            }
        }
    }
}
