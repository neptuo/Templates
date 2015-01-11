
JsTypes.push({
    fullname: "System.IO.StringWriter",
    baseTypeName: "System.Object",
    assemblyName: "SharpKit.JsClr-4.1.0",
    Kind: "Class",
    definition: {
        ctor: function () {
            this.array = null;
            this.length = 0;
            System.Object.ctor.call(this);
            this.array = [];
            this.length = 0;
        }
        ,
        ctor$$String: function (s) {
            this.array = null;
            this.length = 0;
            System.Object.ctor.call(this);
            this.array = [s];
            this.length = s == null ? 0 : s.get_Length();
        }
        ,
        Length$$: "System.Int32",
        get_Length: function () {
            return this.length;
        }

        ,
        set_Length: function (value) {
            if (value != 0)
                throw new System.Exception.ctor$$String("Not Implemented");
            this.array.Clear();
            this.length = value;
        }
        ,
        Write$$Char: function (s) {
            this.array.push(s);
            this.length += s.get_Length();
        }
        ,
        Write$$String: function (s) {
            this.array.push(s);
            this.length += s.get_Length();
        }
        ,
        Write$$Object: function (s) {
            this.array.push(s);
            this.length += s.get_Length();
        }
        ,
        Append$$Object: function (obj) {
            if (obj != null) {
                var s = obj.ToString();
                this.array.push(s);
                this.length += s.get_Length();
            }
        }
        ,
        toString: function () {
            return this.array.join("");
        }
    }
}
);