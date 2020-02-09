# Extensions
# Utilities

## ISwitch.Create<TKey, TValue>

### Usage
    using DNI.Shared.Services;
    var switch = Switch.Create<string,int>()
                        .CaseWhen("foo", 32)
                        .CaseWhen("bar", 42);
    var result = switch.Case("foo"); // returns 32
    var result2 = switch.Case("bar"); // returns 42
## IListBuilder&lt;T>

### Usage

    using DNI.Shared.Services;
    var myListBuilder = ListBuilder.Create<string>()
                            .Add("foo")
                            .Add("bar");

    var list = myListBuilder.ToList(); // returns new string list
    var array = myListBuilder
                .ToEnumerable(); // returns new string IEnumerable
    
## IDictionaryBuilder<TKey, TValue>

### Usage
    
    using DNI.Shared.Services;
    var myDictionaryBuilder = DictionaryBuilder
                                .Create<string, int>()
                                .Add("foo", 32)
                                .Add("bar", 64);

    var dictionary = myDictionaryBuilder
                        .ToDictionary(); //returns dictionary
    