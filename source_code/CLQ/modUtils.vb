Option Explicit On
Namespace Feedback
    Public Class ProgressTracker
#Region "Private Variables"
        Dim Name As New Stack(Of String)
        Dim SubText As New Stack(Of String)
        Dim Total As New Stack(Of Integer)
        Dim Completed As New Stack(Of Integer)
        Dim StartTime As New Stack(Of DateTime)
        Dim lastTimeToComplete As TimeSpan
        Dim dL As Label
        Dim timeLabel As Label
#End Region
#Region "Options"
    Public OneLinePerLevel As Boolean = True
        Public autoDisplay As Boolean = True
        Public forceDisplay As Boolean = True
        Public showTimes As Boolean
#End Region
#Region "Setting up the object"
        Public Sub setLabel(ByVal displayLabel As Label)
            dL = displayLabel
        End Sub
        Public Sub setTimeLabel(ByVal newTimeLabel As Label)
            timeLabel = newTimeLabel
        End Sub
#End Region
#Region "Providing progress information"
        Public Sub initializeTask(ByVal taskName As String)
            Name.Push(taskName)
            SubText.Push("")
            Total.Push(0)
            Completed.Push(0)
            StartTime.Push(Now)
            Display()
        End Sub
        Public Sub finishTask(Optional ByVal taskName As String = "")
            ' taskName is not used- it's purpose is just to make 
            ' the code of the invoking subroutine easier to read

            ' get time of last task
            Dim FinishTime As DateTime = Now
            Dim Start As DateTime = StartTime.Peek
            lastTimeToComplete = FinishTime.Subtract(Start)
            ' remove items from stacks
            Name.Pop()
            SubText.Pop()
            Total.Pop()
            Completed.Pop()
            StartTime.Pop()
            ' display results
            Display()
            If Name.Count = 0 Then dL.Text = "Idle"
        End Sub
        Public Sub changeSubText(ByVal newText As String)
            SubText.Pop()
            SubText.Push(newText)
            Display()
        End Sub
        Public Sub setTotal(ByVal totalCount As Integer)
            Total.Pop()
            Total.Push(totalCount)
            Display()
        End Sub
        Public Sub setCompleted(ByVal numCompleted As Integer)
            Completed.Pop()
            Completed.Push(numCompleted)
            Display()
        End Sub
#End Region
#Region "Displaying results"
        Public Function getText() As String
            Dim nameArray(), subTextArray() As String
            Dim TotalArray(), CompletedArray() As Integer
            Dim startArray() As DateTime
            Dim Finish As DateTime = Now
            Dim elapsed As TimeSpan
            ' get arrays from stacks
            nameArray = Name.ToArray
            subTextArray = SubText.ToArray
            TotalArray = Total.ToArray
            CompletedArray = Completed.ToArray
            startArray = StartTime.ToArray
            ' loop through arrays
            Dim i As Integer, R As String
            R = ""
            For i = nameArray.Count - 1 To 0 Step -1
                If i < nameArray.Count - 1 Then R &= vbCrLf
                R &= nameArray(i)
                If Not OneLinePerLevel Then R &= vbCrLf
                R &= "  " & subTextArray(i)
                If TotalArray(i) > 0 Then R &= CompletedArray(i).ToString & "/" & TotalArray(i).ToString
                If showTimes Then
                    elapsed = Finish.Subtract(startArray(i))
                    R &= " (" & elapsed.TotalSeconds.ToString("F1") & " sec)"
                End If
            Next
            ' return result
            Return R
        End Function
        Public Sub Display()
            If autoDisplay Then
                ' show progress text
                If Not dL Is Nothing Then
                    dL.Text = getText()
                End If
                ' show total time of last task
                If Not timeLabel Is Nothing Then
                    timeLabel.Text = "Last task: " & lastTimeToComplete.TotalSeconds & " sec"
                End If
                ' force display
                If forceDisplay Then Application.DoEvents()
            End If
        End Sub
#End Region
    End Class
    Public Class ErrorChecking
        Shared Function loopCheckExit(ByRef counter As Integer, ByVal errorCheckInterval As Integer) As Boolean
            ' used to avoid endless loops
            ' returns true if the user elects to exit the loop
            ' returns false otherwise
            counter += 1
            If counter Mod errorCheckInterval = 0 Then
                If MsgBox("Haven't found node on part after checking " & _
                           counter.ToString & _
                           " vertices. The program may be in an endless loop. " & _
                           "Do you want to continue?", MsgBoxStyle.OkCancel) = MsgBoxResult.Cancel Then
                    Return True
                End If
            End If
            Return False
        End Function
    End Class
End Namespace
Namespace Data
  Public Class sdsDataBase
    ' an object for establishing a connection with an OLE data source
    '  (an MS Access database is one type of OLE data source)
    Public WithEvents myConn As New System.Data.OleDb.OleDbConnection
    ' an object for storing an SQL command
    '  (SQL - "Standard Query Language" - language for retrieving data from tables)
    Public myComm As New System.Data.OleDb.OleDbCommand
    ' an object for applying an SQL command to a data source to retrieve a data table
    Public myAdapter As New System.Data.OleDb.OleDbDataAdapter
    ' Data Types
    Public Enum dbDataType
      db0_SINGLE = 0             ' single precision; referred to as "real"
      db1_DOUBLE = 1            ' double precision; referred to as "float"
      db2_INTEGER = 2             ' between -2,147,483,648 and 2,147,483,647
      db3_SMALLINT = 3            ' between -32768 and 32767
      db4_TINYINT = 4           ' 0 to 255
      db5_BIT = 5                    ' 0 or 1 (binary)
      db6_TEXT = 6
      db7_DATETIME = 7
    End Enum
    Public Structure dbFieldFormat
      Dim Name As String
      Dim Type As dbDataType
      Dim Length As Integer
    End Structure
    Private Function dataTypeString(ByVal dataType As dbDataType) As String
      Dim R As String = ""
      Select Case dataType
        Case Is = dbDataType.db0_SINGLE
          R = "REAL"
        Case Is = dbDataType.db1_DOUBLE
          R = "FLOAT"
        Case Is = dbDataType.db2_INTEGER
          R = "INTEGER"
        Case Is = dbDataType.db3_SMALLINT
          R = "SMALLINT"
        Case Is = dbDataType.db4_TINYINT
          R = "TINYINT"
        Case Is = dbDataType.db5_BIT
          R = "BIT"
        Case Is = dbDataType.db6_TEXT
          R = "TEXT"
        Case Is = dbDataType.db7_DATETIME
          R = "DATETIME"
        Case Else
          R = "UNDEFINED"
      End Select
      Return R
    End Function
    Public Function loadAccessDatabase(ByVal fileName As String) As Boolean
      ' LOADS A USER-SELECTED MS-ACCESS DATABASE INTO THE DATAGRIDVIEW CONTROL
      ' declare variables
      Dim connStr As String, success As Boolean = True
      ' get connection string for user-selected file
      connStr = accessConnectionString(fileName)
      If connStr <> "" Then
        ' open connection
        myConn.ConnectionString = connStr
        Try
          myConn.Open()
          ' point SQL command handler object to connection
          myComm.Connection = myConn
        Catch ex As Exception
          success = False
        End Try
      Else
        success = False
      End If
      Return success
    End Function
    Public Overloads Function fillTable(ByVal dataGrid As DataGridView, ByVal tableName As String, _
                                Optional ByRef outAdapter As OleDb.OleDbDataAdapter = Nothing, _
                                 Optional ByRef outCommandBuilder As OleDb.OleDbCommandBuilder = Nothing) As Boolean
      ' fills a dataGridView control with a single table from a database
      ' the variables outAdapter and outCommandBuilder will be filled with
      ' objects that can be used to update the database after changes have
      ' been made
      Dim DT As New DataTable
      Dim success As Boolean
      success = fillTable(DT, tableName, outAdapter, outCommandBuilder)
      dataGrid.DataSource = DT
      dataGrid.Refresh()
      Return success
    End Function
    Public Overloads Function fillTable(ByVal DT As DataTable, ByVal tableName As String, _
                                  ByRef outAdapter As OleDb.OleDbDataAdapter, _
                                  ByRef outCommandBuilder As OleDb.OleDbCommandBuilder)
      ' fills a dataTable object with a single table from a database
      ' the variables outAdapter and outCommandBuilder will be filled with
      ' objects that can be used to update the database after changes have
      ' been made
      Dim success As Boolean = True
      Try
        ' database objects
        outAdapter = New System.Data.OleDb.OleDbDataAdapter(sqlGetAll(tableName), myConn)
        outAdapter.SelectCommand.CommandText = sqlGetAll(tableName)
        outCommandBuilder = New OleDb.OleDbCommandBuilder(outAdapter)
        outCommandBuilder.DataAdapter = outAdapter
        ' dataTable variable
        outAdapter.Fill(DT)
      Catch ex As Exception
        success = False
      End Try
      Return success
    End Function
    Public Overloads Function updateTable(ByVal dataGrid As DataGridView, ByRef Adapter As OleDb.OleDbDataAdapter, _
                                            ByRef commandBuilder As OleDb.OleDbCommandBuilder) As Boolean
      ' updates a table in a database with any changes that have been made
      ' by the program or user
      ' the variables Adapter and CommandBuilder come from the fillTable subroutine
      Dim DT As DataTable, success As Boolean = True
      DT = dataGrid.DataSource
      success = updateTable(DT, Adapter, commandBuilder)
      Return success
    End Function
    Public Overloads Function updateTable(ByVal DT As DataTable, ByRef Adapter As OleDb.OleDbDataAdapter, _
                                        ByRef commandBuilder As OleDb.OleDbCommandBuilder) As Boolean
      ' updates a table in a database with any changes that have been made
      ' by the program or user
      ' the variables Adapter and CommandBuilder come from the fillTable subroutine
      Dim success As Boolean = True
      Try
        Adapter.InsertCommand = commandBuilder.GetInsertCommand
        Adapter.DeleteCommand = commandBuilder.GetDeleteCommand
        Adapter.UpdateCommand = commandBuilder.GetUpdateCommand
        Call Adapter.Update(DT)
      Catch ex As Exception
        success = False
      End Try
      Return success
    End Function
    Public Sub executeSQL(ByVal sqlCommand As String)
      ' executes an SQL query without returning any data
      myComm.CommandText = sqlCommand
      myComm.ExecuteNonQuery()
    End Sub
    Public Function SQLqueryResult(ByVal sqlCommand As String) As DataTable
      ' executes an SQL query and returns a data table with the results
      Dim R As New DataTable
      ' set up the OleDbCommand object to retrieve the data
      myComm.CommandText = sqlCommand ' give it the SQL command
      ' create an OleDbDataAdapter to transfer data from the data source into a data table
      myAdapter = New System.Data.OleDb.OleDbDataAdapter(sqlCommand, myConn)

      ' transfer the data from the data source into a data table
      myAdapter.Fill(R)
      ' return result
      Return R
    End Function
    Private Function accessConnectionString(ByVal AccessFileName As String) As String
      ' creates text to define a basic database connection
      ' input should be a Microsoft Access database file
      Dim Access2007 As Boolean, R As String, fileExt As String
      R = ""
      ' check to see what version this is
      fileExt = Right(AccessFileName, 5)
      If fileExt = "accdb" Then
        Access2007 = True
      Else
        fileExt = Right(AccessFileName, 3)
      End If
      If Access2007 Then
        R = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & AccessFileName & ";"
      ElseIf fileExt = "mdb" Then
        R = "Provider=Microsoft.Jet.Oledb.4.0;"
        R = R + "Data Source=" & AccessFileName & ";"
      Else
        MsgBox("Sorry, currently only MS Access databases are supported.")
      End If
      Return R
    End Function
    Public Function sqlGetAll(ByVal tableName As String) As String
      ' creates an SQL command to retrieve an entire data table
      Return "Select * from " & tableName
    End Function
    Public Function sqlCreateTable(ByVal tableName As String, _
                                   ByVal dataField() As dbFieldFormat, _
                                   Optional ByVal primaryKeyField As Integer = -1) _
                                   As String
      ' creates an SQL query to create a data table
      Dim R As String, curField As Integer = 0
      ' error checking
      Dim errorStr As String = ""
      If primaryKeyField < -1 Then errorStr &= "Primary Key Field was less than -1..."
      If primaryKeyField > UBound(dataField) Then errorStr &= "Primary Key Field was greater than the number of fields..."
      ' start
      R = "CREATE TABLE " & tableName & " ("
      Do
        ' field name
        If dataField(curField).Name = "" Then
          errorStr &= "Data field " & Str(curField) & " has no name; default name was given..."
          dataField(curField).Name = "Field" & Str(curField)
        End If
        R &= dataField(curField).Name & " "
        ' data type
        R &= dataTypeString(dataField(curField).Type) & " "
        ' length (text fields only)
        If dataField(curField).Type = dbDataType.db6_TEXT Then
          If dataField(curField).Length < 1 Then
            dataField(curField).Length = 255
            errorStr &= "Data field " & Str(curField) & " is a text field but maximum size was not specified; default size of 255 was given..."
          End If
          R &= "(" & CStr(dataField(curField).Length) & ")"
        End If
        ' primary key
        If primaryKeyField = curField Then
          R &= "PRIMARY KEY "
        End If
        ' remove last space
        R = Left(R, Len(R) - 1)
        ' final comma
        If curField <> UBound(dataField) Then R &= ", "
        curField += 1
      Loop Until curField > UBound(dataField)
      ' add final parentheses
      R &= ")"
      ' RETURN RESULT
      Return R
    End Function
    Public Function sqlInsertInto(ByVal TableName As String, _
                                              ByVal FieldName() As String, _
                                              ByVal Value() As String) As String
      ' generates an SQL query to insert new data records into a data table
      Dim R As String, I As Integer
      ' ERROR CATCHING - just make sure array lengths match!
      ' BASE
      R = "INSERT INTO " & TableName & " "
      ' FIELD NAMES
      R &= "("
      For I = 0 To UBound(FieldName)
        R &= FieldName(I)
        If I = UBound(FieldName) Then R &= ") " Else R &= ", "
      Next
      ' VALUES
      R &= "VALUES ("
      For I = 0 To UBound(Value)
        R &= "'" & Value(I) & "'"
        If I = UBound(FieldName) Then R &= ")" Else R &= ", "
      Next
      ' RETURN RESULT
      Return R
    End Function
    Public Function TableNameList() As String()
      ' returns a list of tables from an MS Access database
      Dim Result() As String
      Dim NumTables As Integer = 0
      Dim curRow As System.Data.DataRow, curType As String
      Dim nameCol, typeCol As Integer
      Dim tableOfTables As System.Data.DataTable
      ' set result default to avoid warning in Error List
      ReDim Result(0 To 0)
      ' get list of tables from database schema
      tableOfTables = myConn.GetSchema("Tables")
      ' get columns for table name & type
      nameCol = tableOfTables.Columns.IndexOf("TABLE_NAME")
      typeCol = tableOfTables.Columns.IndexOf("TABLE_TYPE")
      ' go through list of tables
      For Each curRow In tableOfTables.Rows
        ' get table type
        curType = curRow.Item(typeCol)
        ' we only need the REAL tables that the MS Access user would see
        If curType = "TABLE" Then
          NumTables = NumTables + 1
          ReDim Preserve Result(0 To NumTables - 1)
          Result(NumTables - 1) = curRow.Item(nameCol)
        End If
      Next
      Return Result
    End Function
  End Class
  Public Class Sorting
    ' provides methods to create a sorted index from an unsorted array of numbers
    ' 1. Use function SortIndex to return an index in the form of an array
    '    Example: suppose you have an array X().  Then after 
    '    you run the following 3 lines:
    '     Dim S as new Sorter
    '     Dim I() as Integer
    '     I = S.SortIndex(X)
    '    the array element I(3) will give the index of the 3rd lowest element in X
    ' 2. Use function Rank to determine the rank of each item from the index I
    '    Continuing the example above, after you execute the following 2 lines:
    '     Dim R() as Integer
    '     R = Rank(I)
    '    Then the value of R(3) will tell you the rank of X(3).  So, if R(3)=95, 
    '    that means that X(3) is the 95th lowest element of X.

    Shared Sub compareValueToArray(ByVal Value As Double, _
                             ByVal inArray() As Double, _
                             ByRef numLower As Integer, _
                             ByRef numSame As Integer, _
                             ByRef numHigher As Integer)
      numLower = 0
      numSame = 0
      numHigher = 0
      For i = 0 To inArray.Length - 1
        Select Case Value
          Case Is < inArray(i)
            numLower += 1
          Case Is = inArray(i)
            numSame += 1
          Case Is > inArray(i)
            numHigher += 1
        End Select
      Next
    End Sub
    Shared Function Rank(ByVal IndexOfRank() As Integer) As Integer()
      ' returns the rank of each item based on an index of ranks
      Dim R() As Integer, i As Integer
      ReDim R(UBound(IndexOfRank))
      For i = 0 To UBound(R)
        R(IndexOfRank(i)) = i
      Next i
      Rank = R
    End Function
    Private Function searchLow(ByVal Target As Double, ByVal X() As Double, ByVal xIndex() As Integer) As Integer
      Dim Low As Integer, High As Integer, Pivot As Integer
      ' initialize
      Low = 0
      High = UBound(X)
      Pivot = Int((Low + High) / 2)
      ' loop
      Do While (High - Low) > 1
        If Target > X(xIndex(Pivot)) Then
          Low = Pivot
        Else
          High = Pivot
        End If
        Pivot = Int((Low + High) / 2)
      Loop
      ' return result
      searchLow = Low
    End Function
    Shared Function SortIndex(ByVal X() As Double) As Integer()
      ' returns an array of indices R() such that :
      ' R(0) is the index of the lowest value
      ' R(1) is the index of the second lowest value
      ' etc.
      Dim finish As Integer, count As Integer
      Dim resultIndexOfRank() As Integer
      Dim meExplicit As New Sorting
      ' initialize
      Dim i As Integer
      ReDim resultIndexOfRank(UBound(X))
      count = UBound(X) + 1
      For i = 0 To count - 1
        resultIndexOfRank(i) = i
      Next
      ' first place X in max-heap order
      Call meExplicit.heapify(X, resultIndexOfRank, count)
      ' start with last item
      finish = count - 1
      ' loop
      Do While finish > 0
        ' swap the root with the last element
        Call meExplicit.swap(resultIndexOfRank, finish, 0)
        ' decrease the size of the heap by one so that
        ' the previous max value will stay in its proper place
        finish = finish - 1
        ' put the heap back in max-heap order
        Call meExplicit.siftDown(X, resultIndexOfRank, 0, finish)
      Loop
      ' return result
      SortIndex = resultIndexOfRank
    End Function
    Private Sub heapify(ByRef X() As Double, _
                        ByRef resultIndexOfRank() As Integer, _
                        ByVal count As Integer)
      Dim start As Long
      ' start is assigned the index of the last parent node
      start = (count - 1) / 2
      ' loop
      Do While start >= 0
        ' sift down the node at start position to the proper place
        ' so that all the start indices are in heap order
        Call siftDown(X, resultIndexOfRank, start, count - 1)
        start = start - 1
      Loop
    End Sub
    Private Sub siftDown(ByRef X() As Double, _
                         ByRef resultIndexOfRank() As Integer, _
                         ByVal start As Long, ByRef finish As Integer)
      ' input value "finish" represents the limit of how far down the heap to sift
      Dim root As Integer, child As Integer
      ' initialize
      root = start
      ' loop
      Do While root * 2 + 1 <= finish ' while the root has at least one child
        child = root * 2 + 1        ' this points to the left child
        ' if the child has a sibling and the child's value is less than it's siblings
        If child < finish Then
          If X(resultIndexOfRank(child)) < X(resultIndexOfRank(child + 1)) Then child = child + 1 ' point to the right child
        End If
        If X(resultIndexOfRank(root)) < X(resultIndexOfRank(child)) Then ' out of max-heap order
          Call swap(resultIndexOfRank, root, child)
          root = child
        Else
          Exit Sub
        End If
      Loop
    End Sub
    Private Sub swap(ByRef resultIndexOfRank() As Integer, _
                     ByVal p1 As Long, ByVal p2 As Integer)
      Dim t
      t = resultIndexOfRank(p1)
      resultIndexOfRank(p1) = resultIndexOfRank(p2)
      resultIndexOfRank(p2) = t
    End Sub
    Shared Sub shuffleInteger(ByRef intArray() As Integer)
      ' shuffles and array of the same integers
      ' creates a random array of the integers from 0 to listLength-1
      ' this is a linear time algorithm
      Dim i As Long
      Dim swapPos As Integer, tempVal As Integer
      Dim listLength As Integer = intArray.Count
      Randomize()
      ' loop through items to rearrange list
      For i = 0 To listLength - 1
        ' pick a random number from i to UBound(memberClass)
        swapPos = i + Int(Rnd() * (listLength - i))
        ' swap values
        tempVal = intArray(i)
        intArray(i) = intArray(swapPos)
        intArray(swapPos) = tempVal
      Next i
    End Sub
    Shared Function getShuffledInteger(ByVal intArray() As Integer) As Integer()
      ' returns a shuffled array of the same integers
      Dim RO() As Integer
      Dim R() As Integer
      Dim i As Integer
      ' set up results array
      ReDim R(UBound(intArray))
      ' get random order
      RO = randomOrder(intArray.Count)
      ' exchange values
      For i = 0 To intArray.Count - 1
        R(i) = intArray(RO(i))
      Next
      ' return result
      Return R
    End Function
    Shared Function randomOrder(ByVal listLength As Integer) As Integer()
      ' creates a random array of the integers from 0 to listLength-1
      ' this is a linear time algorithm
      Dim R() As Integer, i As Long
      Dim swapPos As Long, tempVal As Integer
      ReDim R(listLength - 1)
      Randomize()
      ' loop through items once to create array of integers
      For i = 0 To listLength - 1
        R(i) = i
      Next
      ' loop through items again to rearrange list
      For i = 0 To listLength - 1
        ' pick a random number from i to UBound(memberClass)
        swapPos = i + Int(Rnd() * (listLength - i))
        ' swap values
        tempVal = R(i)
        R(i) = R(swapPos)
        R(swapPos) = tempVal
      Next i
      ' return result
      Return R
    End Function
    Shared Function sequenceVector(ByVal numElements As Integer) As Integer()
      Dim i As Integer
      Dim R As Integer()
      ReDim R(numElements - 1)
      For i = 0 To numElements - 1
        R(i) = i
      Next
      Return R
    End Function
  End Class
  Public Class Numbers
    Overloads Shared Function maxVal(ByVal v1 As Double, ByVal v2 As Double) As Double
      If v1 > v2 Then Return v1 Else Return v2
    End Function
    Overloads Shared Function maxVal(ByVal v1 As Double, ByVal v2 As Double, ByVal v3 As Double) As Double
      Dim max12 As Double = maxVal(v1, v2)
      Return maxVal(max12, v3)
    End Function
    Overloads Shared Function maxVal(ByVal v1 As Double, ByVal v2 As Double, ByVal v3 As Double, ByVal v4 As Double) As Double
      Dim max12, max34 As Double
      max12 = maxVal(v1, v2)
      max34 = maxVal(v3, v4)
      Return maxVal(max12, max34)
    End Function
    Overloads Shared Function maxVal(ByVal v()) As Double
      Dim i As Integer, M As Double
      If v Is Nothing Then Return Nothing
      M = v(0)
      For i = 1 To v.Count - 1
        If v(i) > M Then M = v(i)
      Next
      Return M
    End Function
    Shared Sub compareValueToArray(ByVal inVal As Double, _
                                   ByVal inArray() As Double, _
                                   ByRef numLower As Integer, _
                                   ByRef numSame As Integer, _
                                   ByRef numHigher As Integer)
      ' calculates the number of elements of the input array
      ' that are lower, the same and higher than the input value

      ' reset results
      numLower = 0
      numSame = 0
      numHigher = 0
      ' loop through array elements
      For Each ArrayVal In inArray
        ' compare
        Select Case ArrayVal
          Case Is < inVal
            numLower += 1
          Case Is = inVal
            numSame += 1
          Case Is > inVal
            numHigher += 1
        End Select
      Next
    End Sub
  End Class
  Public Class Lookup
    Shared Function uniqueValueList(ByVal values() As Object) As List(Of Object)
      ' returns a list of the unique objects from the input
      Dim R As New List(Of Object)
      For i = 1 To values.Length
        If R.IndexOf(values(i - 1)) < 0 Then
          R.Add(values(i - 1))
        End If
      Next
      Return R
    End Function
    Shared Sub getIntegerIDswithLookup(ByVal v() As Object, _
                                       ByRef classID() As Integer, _
                                       ByRef classNames() As Object, _
                                       ByRef classIDLookup As IDictionary)
      ' input must be integer or string

      ' creates:
      '  (a) integer class ID for each point
      '  (b) dictionary of objects for each class ID
      '  (c) reverse dictionary (if objects are strings or integers)
      Dim objStr As Boolean = False, objInt As Boolean = False, doDict As Boolean = False
      ' see if they're all the same type
      Dim allSameType As Boolean = True
      For i = 1 To v.Length - 1
        If Not v(i).GetType.IsEquivalentTo(v(0).GetType) Then allSameType = False : Exit For
      Next
      If allSameType Then
        Dim intType As Integer = 0
        Dim strType As String = ""
        If v(0).GetType.IsEquivalentTo(intType.GetType) Then
          objInt = True : doDict = True
        Else
          If v(0).GetType.IsEquivalentTo(strType.GetType) Then objStr = True : doDict = True
        End If
      End If
      If Not doDict Then Exit Sub
      ' get list of class names
      Dim cNameList As List(Of Object), cItemCount As List(Of Integer)
      calcUniqueValueCounts(v, cNameList, cItemCount)
      classNames = cNameList.ToArray
      ' get reverse lookup dictionary if string or text
      If doDict Then
        If objInt Then classIDLookup = New Dictionary(Of Integer, Integer)
        If objStr Then classIDLookup = New Dictionary(Of String, Integer)
        For i = 0 To classNames.Length - 1
          classIDLookup.Add(classNames(i), i)
        Next
      End If
      ' get classes
      ReDim classID(UBound(v))
      For i = 0 To UBound(v)
        classID(i) = classIDLookup(v(i))
      Next
      ' you're done!
    End Sub
    Shared Sub calcUniqueValueCounts(ByVal values() As Object, _
                                 ByRef uniqueValList As List(Of Object), _
                                 ByRef valueCount As List(Of Integer))
      ' determines a list of the unique objects from the input
      ' additionally tallies the number of objects of each value

      ' first get observed NNCT


      ' more efficient if we sort!
      Dim canSort As Boolean = True
      ' see if they're all the same type
      For i = 1 To values.Length - 1
        If Not values(i).GetType.IsEquivalentTo(values(0).GetType) Then
          canSort = False
          Exit For
        End If
      Next
      If canSort Then
        ' get type of object
        Dim intType As Integer = 0
        Dim strType As String = ""
        If values(0).GetType.IsEquivalentTo(intType.GetType) Then
          Dim intVal() As Integer
          ReDim intVal(UBound(values))
          values.CopyTo(intVal, 0)
          System.Array.Sort(intVal)
          For i = 0 To UBound(values)
            values(i) = intVal(i)
          Next
        ElseIf values(0).GetType.IsEquivalentTo(strType.GetType) Then
          Dim strVal() As String
          ReDim strVal(UBound(values))
          values.CopyTo(strVal, 0)
          System.Array.Sort(strVal)
          values = strVal
        Else
          canSort = False
        End If ' types are equivalent
      End If ' canSort

      ' set up variables
      uniqueValList = New List(Of Object)
      valueCount = New List(Of Integer)
      Dim curCat As Integer
      Dim curCatCount As Integer = 0
      Dim curCatVal As Object
      ' Use different technique depending on whether or not we can sort:
      If canSort Then
        ' Do it the efficient way (values array is already sorted):
        curCat = 0
        uniqueValList.Add(values(0))
        curCatCount += 1
        curCatVal = values(0)
        For i = 1 To values.Length - 1
          If values(i) <> curCatVal Then
            ' create new category
            curCatVal = values(i)
            curCat += 1
            uniqueValList.Add(curCatVal)
            ' record count of last category and reset counter
            valueCount.Add(curCatCount)
            curCatCount = 0
          End If
          ' add to count
          curCatCount += 1
        Next
        ' record count of last category

        valueCount.Add(curCatCount)
      Else
        ' If we can't sort, do it the inefficient way (accesses list values often):    
        For i = 0 To values.Length - 1
          curCat = uniqueValList.IndexOf(values(i)) ' this line is inefficient
          If curCat < 0 Then
            uniqueValList.Add(values(i))
            valueCount.Add(0)
            curCat = uniqueValList.Count - 1
          End If
          valueCount.Item(curCat) = valueCount.Item(curCat) + 1 ' this line is ineffecient
        Next

      End If
    End Sub
    Shared Sub createIDLookup(ByVal inValues() As Object, _
                            ByRef outIDs() As Integer, _
                            ByRef outIDLookup As List(Of Object))
      ' converts a set of category values to a set of category IDs,
      ' numbered 0 to k-1 (where k is the number of categories)
      ' outIDLookup is a lookup table giving the value of each category ID

      ' set up result variables
      outIDLookup = New List(Of Object)
      ReDim outIDs(inValues.Length - 1)
      ' iterate
      For i = 0 To inValues.Length - 1
        If outIDLookup.IndexOf(inValues(i)) < 0 Then outIDLookup.Add(inValues(i))
        outIDs(i) = outIDLookup.IndexOf(inValues(i))
      Next
    End Sub
    Shared Function columnArray(ByVal DT As DataTable, ByVal ColumnName As String) As Object()
      ' returns an array of values from specified column of data table
      Dim R() As Object
      ReDim R(DT.Rows.Count - 1)
      Dim columnID As Integer = DT.Columns.IndexOf(ColumnName)
      For i = 0 To DT.Rows.Count - 1
        R(i) = DT.Rows.Item(i).Item(columnID)
      Next
      Return R
    End Function
  End Class
End Namespace
Namespace Spatial
  Public Class MapDrawer
    ' Provides methods to draw points, lines and polygons
    ' onto a PictureBox control
    ' To use:
    ' 1. Declare and instantiate a variable of type sdsMapDraw
    ' 2. Use the LinkToPictureBox subroutine to connect to a PictureBox
    ' 3. Use the drawPoints, drawLine, drawPolygon and drawText subroutines to
    '    draw on the picture box.
    Private picMap As PictureBox
    Public UnitsPerPixel As Single = 1
    Public XOrigin As Single = 0
    Public YOrigin As Single = 0
    Private G As Graphics
    Public Sub LinkToPictureBox(ByVal picBox As PictureBox)
      ' sets up a graphic link to a picture box
      ' Note: this subroutine needs to be called whenever the picture box is resized
      picMap = picBox
    End Sub
    Private Function picX(ByVal mapX As Single) As Single
      picX = (mapX - XOrigin) / UnitsPerPixel
    End Function
    Private Function picY(ByVal mapY As Single) As Single
      picY = picMap.Height - (mapY - YOrigin) / UnitsPerPixel
    End Function
    Public Sub resetGraphics()
      If picMap Is Nothing Then Exit Sub
      Dim bmp As Bitmap
      bmp = New Bitmap(picMap.Width, picMap.Height)
      picMap.Image = bmp
      G = Graphics.FromImage(bmp)
      G.Clear(picMap.BackColor)
    End Sub
    Public Sub drawPoints(ByVal ptX() As Single, _
                          ByVal ptY() As Single, _
                           ByVal outlineColor As System.Drawing.Color, _
                           ByVal outlineWidth As Integer, _
                           ByVal fillColor As Color, _
                           ByVal pixelWidth As Single)
      Dim pPen As New Pen(outlineColor)
      pPen.Width = outlineWidth
      Dim pBrush As Brush = New SolidBrush(fillColor)
      Dim i As Integer
      Dim pX, pY As Single
      If G Is Nothing Then Exit Sub
      For i = 0 To UBound(ptX)
        pX = picX(ptX(i)) - pixelWidth / 2
        pY = picY(ptY(i)) - pixelWidth / 2
        G.FillEllipse(pBrush, pX, pY, pixelWidth, pixelWidth)
        G.DrawEllipse(pPen, pX, pY, pixelWidth, pixelWidth)
      Next i
    End Sub
    Public Sub drawLine(ByVal lineX() As Single, _
                                 ByVal lineY() As Single, _
                                 ByVal lineColor As Color, _
                                 ByVal lineWidth As Integer)
      Dim curPT(UBound(lineX)) As System.Drawing.Point
      Dim pX, pY As Single
      Dim i As Integer
      Dim pPen As New Pen(lineColor)
      pPen.Width = lineWidth
      For i = 0 To UBound(lineX)
        pX = picX(lineX(i))
        pY = picY(lineY(i))
        curPT(i) = New System.Drawing.Point
        curPT(i).X = pX
        curPT(i).Y = pY
      Next
      Call G.DrawLines(pPen, curPT)
    End Sub
    Public Sub drawPolygon(ByVal polyX() As Single, _
                           ByVal polyY() As Single, _
                           ByVal lineColor As Color, _
                           ByVal lineWidth As Integer, _
                           ByVal fillColor As Color)
      Dim pPen As New Pen(lineColor)
      pPen.Width = lineWidth
      Dim pBrush As New SolidBrush(fillColor)
      Dim curPT(UBound(polyX)) As System.Drawing.Point
      Dim pX, pY As Single
      Dim i As Integer
      For i = 0 To UBound(polyX)
        pX = picX(polyX(i))
        pY = picY(polyY(i))
        curPT(i) = New System.Drawing.Point
        curPT(i).X = pX
        curPT(i).Y = pY
      Next
      G.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
      G.FillPolygon(pBrush, curPT)
      G.DrawPolygon(pPen, curPT)
    End Sub
    Public Sub drawText(ByVal textString As String, ByVal xOrigin As Single, ByVal yOrigin As Single, ByVal textFont As String, ByVal size As Single)
      Dim pX, pY As Single
      Dim pFontFamily As FontFamily = New FontFamily(textFont)
      Dim pFont As Font = New Font(pFontFamily, size)
      pX = picX(xOrigin)
      pY = picY(yOrigin)
      G.DrawString(textString, pFont, Brushes.Black, pX, pY)
    End Sub
  End Class
  Public Class ShapefileUtils
    Public Structure SFPtInfo
      Dim ShpID As Integer
      Dim PartNum As Integer
      Dim PtID As Integer
      Dim X As Double
      Dim Y As Double
      Dim nextID As Integer
      Dim prevID As Integer
    End Structure
    'Shared Function numPointsInPart(ByVal SHP As Shape, ByVal PartNum As Integer) As Integer
    '  ' error checking
    '  ' note that a shapefile with only one part may say it has
    '  ' zero or one parts
    '  If PartNum < 0 Then Return -1
    '  If SHP.NumParts = 0 Then
    '    If PartNum > 0 Then Return -1
    '  Else
    '    If PartNum > SHP.NumParts - 1 Then Return -1
    '  End If
    '  ' deal with cases
    '  Select Case SHP.NumParts
    '    Case Is < 2
    '      Return SHP.numPoints
    '    Case Is = PartNum + 1
    '      Return SHP.numPoints - SHP.Part(PartNum)
    '    Case Else
    '      Return SHP.Part(PartNum + 1) - SHP.Part(PartNum)
    '  End Select
    '  ' just in case the above cases don't cover everything:
    '  Return -1
    'End Function
    'Shared Function noRepeatPolySF(ByVal polySF As Shapefile, _
    '                Optional ByVal P As Feedback.ProgressTracker = Nothing, _
    '                Optional ByVal UpdateIncrement As Integer = 100) As Shapefile
    '  ' creates a new shapefile with all of the points from the input shapefile
    '  ' except duplicate points where the last point in a polygon part
    '  ' has the same coordinates as the first point
    '  ' *Warning: the result is not a "proper" shapefile!
    '  Dim R As New Shapefile
    '  Dim curPolyID, curPartID, curPtID As Integer
    '  Dim curPoly, newPoly As Shape, curPT, newPT As MapWinGIS.Point
    '  Dim firstPtInPartID, lastPtInPartID As Integer
    '  Dim firstPT As MapWinGIS.Point = Nothing
    '  Dim skipPT As Boolean
    '  Dim numSkipped As Integer
    '  ' initialize
    '  If Not P Is Nothing Then
    '    P.initializeTask("Removing redundant points...")
    '    P.setTotal(polySF.NumShapes)
    '  End If
    '  R = New Shapefile
    '  R.CreateNew("", ShpfileType.SHP_POLYGON)
    '  ' FIRST COUNT THE NUMBER OF POINTS IN THE ENTIRE SHAPEFILE
    '  For curPolyID = 0 To polySF.NumShapes - 1
    '    ' retrieve old polygon, create new one
    '    curPoly = polySF.Shape(curPolyID)
    '    newPoly = New Shape
    '    newPoly.Create(ShpfileType.SHP_POLYGON)
    '    ' initialize first part
    '    curPartID = 0
    '    firstPtInPartID = 0
    '    If curPoly.NumParts = 1 _
    '      Then lastPtInPartID = curPoly.numPoints _
    '      Else lastPtInPartID = curPoly.Part(1) - 1
    '    ' initialize other stuff
    '    numSkipped = 0
    '    ' loop through points
    '    For curPtID = 0 To curPoly.numPoints - 1
    '      ' initialize
    '      skipPT = False
    '      curPT = curPoly.Point(curPtID)
    '      newPT = New MapWinGIS.Point
    '      newPT.x = curPT.x
    '      newPT.y = curPT.y
    '      ' check if we've moved on to a new part
    '      If curPtID = lastPtInPartID + 1 Then
    '        curPartID += 1
    '        firstPtInPartID = curPtID
    '        If curPartID = curPoly.NumParts - 1 _
    '          Then lastPtInPartID = curPoly.numPoints _
    '          Else lastPtInPartID = curPoly.Part(curPartID + 1) - 1
    '      End If
    '      '  if this is the first point in the part record it
    '      If curPtID = firstPtInPartID Then
    '        firstPT = curPT
    '        newPoly.InsertPart(curPtID - numSkipped, curPartID)
    '      End If
    '      ' check if point is last in part
    '      If curPtID = lastPtInPartID Then
    '        ' make sure there's not just one point in the part
    '        If Not firstPtInPartID = lastPtInPartID Then
    '          ' see if coordinates are the same as the first point in the part
    '          If (curPT.x = firstPT.x) And (curPT.y = firstPT.y) Then
    '            skipPT = True
    '            numSkipped += 1
    '          End If ' coords same as 1st pt
    '        End If ' not just one point in part
    '      End If ' point is last in part
    '      ' add point to shape
    '      If Not skipPT Then
    '        newPoly.InsertPoint(newPT, newPoly.numPoints)
    '      End If
    '    Next curPtID
    '    ' insert polygon into shapefile
    '    R.EditInsertShape(newPoly, R.NumShapes)
    '    ' report progress
    '    If (curPolyID + 1) Mod UpdateIncrement = 0 Then
    '      If Not P Is Nothing Then P.setCompleted(curPolyID + 1)
    '    End If
    '  Next curPolyID
    '  ' report finish
    '  If Not P Is Nothing Then P.finishTask()
    '  ' return new shapefile
    '  Return R
    'End Function
    'Shared Function shapefileCopy(ByVal inSF As Shapefile, _
    '                       Optional ByVal fieldsToCopy As Integer() = Nothing, _
    '                       Optional ByVal addAreaField As Boolean = False, _
    '                       Optional ByVal P As Feedback.ProgressTracker = Nothing, _
    '                       Optional ByVal updateInterval As Integer = 10) _
    '                       As Shapefile
    '  ' make copy of shapefile, to make sure we have memory access
    '  Dim sfCopy As Shapefile
    '  Dim curField As Integer
    '  Dim curShpID As Integer
    '  ' show progress
    '  If Not P Is Nothing Then
    '    ' initialize general progress
    '    P.initializeTask("Copying metadata...")
    '    ' move to first task
    '    P.initializeTask("Initializing...")
    '  End If
    '  '   create shapefile
    '  sfCopy = New Shapefile
    '  sfCopy.CreateNew("", inSF.ShapefileType)
    '  ' get fields
    '  If fieldsToCopy Is Nothing Then
    '    ReDim fieldsToCopy(inSF.NumFields - 1)
    '    For curField = 0 To inSF.NumFields - 1
    '      fieldsToCopy(curField) = curField
    '    Next
    '  End If
    '  ' add fields
    '  For curField = 0 To fieldsToCopy.Count - 1
    '    sfCopy.EditInsertField(inSF.Field(fieldsToCopy(curField)), curField)
    '  Next
    '  If addAreaField Then
    '    Dim areaField As New Field
    '    areaField.Name = "Area"
    '    areaField.Type = FieldType.DOUBLE_FIELD
    '    areaField.Width = 32
    '    sfCopy.EditInsertField(areaField, fieldsToCopy.Count)
    '  End If
    '  ' move to next task
    '  If Not P Is Nothing Then
    '    ' end initialization task
    '    P.finishTask()
    '    ' move to copying task
    '    P.initializeTask("Copying...")
    '    P.changeSubText("Polygon #")
    '    P.setTotal(inSF.NumShapes)
    '  End If
    '  ' loop through shapes
    '  For curShpID = 0 To inSF.NumShapes - 1
    '    ' polygon shape
    '    sfCopy.EditInsertShape(inSF.Shape(curShpID), curShpID)
    '    ' fields 
    '    For curField = 0 To fieldsToCopy.Count - 1
    '      sfCopy.EditCellValue(curField, curShpID, inSF.CellValue(fieldsToCopy(curField), curShpID))
    '    Next
    '    ' area field
    '    'If addAreaField Then
    '    '  sfCopy.EditCellValue(fieldsToCopy.Count, _
    '    '                       curShpID, _
    '    '                       MapWinGeoProc.Utils.Area(sfCopy.Shape(curShpID)))
    '    'End If
    '    ' report progress
    '    If Not P Is Nothing Then
    '      If (curShpID + 1) Mod updateInterval = 0 Then
    '        P.setCompleted(curShpID + 1)
    '      End If
    '    End If
    '  Next curShpID
    '  ' report finish
    '  If Not P Is Nothing Then
    '    P.finishTask() ' copying
    '    P.finishTask() ' outer task
    '  End If
    '  ' return result
    '  Return sfCopy
    'End Function
    'Shared Function indexOfPoints(ByVal SF As Shapefile, _
    '                Optional ByVal removeRedundantPoints As Boolean = True, _
    '                Optional ByVal P As Feedback.ProgressTracker = Nothing, _
    '                Optional ByVal updateIncrement As Integer = 100) _
    '                As SFPtInfo()
    '  ' creates an array of information about each point in the 
    '  ' input polygon shapefile
    '  Dim inSF As Shapefile
    '  Dim R() As SFPtInfo
    '  Dim numPTs As Integer = 0
    '  Dim newPtNum As Integer
    '  Dim curShpID, curPartID, curPtID As Integer
    '  Dim curSHP As Shape, curPT As MapWinGIS.Point
    '  Dim partStartID As Integer
    '  Dim numPtsInPart As Integer
    '  ' report start
    '  If Not P Is Nothing Then
    '    P.initializeTask("Indexing points in shapefile...")
    '    P.changeSubText("Copying shapefile...")
    '  End If

    '  ' get shapefile
    '  If removeRedundantPoints Then
    '    inSF = noRepeatPolySF(SF, P)
    '  Else
    '    inSF = SF
    '  End If
    '  ' figure out size of array
    '  numPTs = 0
    '  For curShpID = 0 To inSF.NumShapes - 1
    '    curSHP = inSF.Shape(curShpID)
    '    numPTs += curSHP.numPoints
    '  Next
    '  ' size output array
    '  ReDim R(numPTs - 1)
    '  ' report progress
    '  If Not P Is Nothing Then
    '    P.changeSubText("Polygon ")
    '    P.setTotal(SF.NumShapes)
    '  End If
    '  ' loop through polygons
    '  newPtNum = 0
    '  For curShpID = 0 To inSF.NumShapes - 1
    '    ' initialize polygon
    '    curSHP = inSF.Shape(curShpID)
    '    curPartID = 0
    '    partStartID = newPtNum
    '    ' loop through points
    '    For curPtID = 0 To curSHP.numPoints - 1
    '      ' see if we're at a new part
    '      If (curSHP.NumParts > curPartID + 1) And (curSHP.Part(curPartID + 1) = curPtID) Then
    '        curPartID += 1
    '        partStartID = newPtNum
    '      End If
    '      ' get basic info
    '      R(newPtNum).ShpID = curShpID
    '      R(newPtNum).PartNum = curPartID
    '      R(newPtNum).PtID = curPtID
    '      curPT = curSHP.Point(curPtID)
    '      R(newPtNum).X = curPT.x
    '      R(newPtNum).Y = curPT.y
    '      ' NEXT SECTION CREATES FORWARD/BACKWARD LINKS
    '      ' HIGH POSSIBILITY OF CODING ERROR!
    '      ' get ID of last point in part
    '      If curSHP.Part(curPartID) = curPtID Then
    '        ' first point in part
    '        numPtsInPart = Spatial.ShapefileUtils.numPointsInPart(curSHP, curPartID)
    '        R(newPtNum).prevID = newPtNum + numPtsInPart - 1
    '      Else
    '        R(newPtNum).prevID = newPtNum - 1
    '      End If
    '      ' get ID of next point in part
    '      If (curPtID + 1 = curSHP.Part(curPartID + 1)) Or (curPtID = curSHP.numPoints - 1) Then
    '        ' last point in part
    '        R(newPtNum).nextID = partStartID
    '      Else
    '        R(newPtNum).nextID = newPtNum + 1
    '      End If
    '      ' increment point number
    '      newPtNum += 1
    '    Next curPtID
    '    ' report progress
    '    If (curShpID + 1) Mod updateIncrement = 0 Then
    '      If Not P Is Nothing Then P.setCompleted(curShpID + 1)
    '    End If
    '  Next curShpID
    '  ' report finish
    '  If Not P Is Nothing Then P.finishTask()
    '  ' Return Result
    '  Return R
    'End Function
    Shared Sub displayPtIndex(ByVal ptIndex() As SFPtInfo, _
                              ByVal Dat As DataGridView, _
                              Optional ByVal P As Feedback.ProgressTracker = Nothing, _
                              Optional ByVal updateIncrement As Integer = 1000)
      ' DISPLAYS AN ARRAY OF INFORMATION 
      ' ABOUT THE POINTS IN A SHAPEFILE
      ' IN A DATAGRIDVIEW CONTROL
      Dim R As Integer
      ' report start
      If Not P Is Nothing Then
        P.initializeTask("Filling in table...")
        P.setTotal(ptIndex.Count)
        P.changeSubText("Setting up columns...")
      End If
      ' clear out dataGridView just in case
      Dat.DataSource = Nothing
      ' set up rows and columns
      Dat.RowCount = ptIndex.Count
      Dat.ColumnCount = 6
      Dat.ColumnHeadersVisible = True
      Dat.RowHeadersVisible = False
      ' set up column headers
      Dat.Columns(0).HeaderText = "ID"
      Dat.Columns(1).HeaderText = "Shape"
      Dat.Columns(2).HeaderText = "Part"
      Dat.Columns(3).HeaderText = "Pt"
      Dat.Columns(4).HeaderText = "Next"
      Dat.Columns(5).HeaderText = "Prev"
      ' report progress
      If Not P Is Nothing Then P.changeSubText("Inserting rows...")
      ' loop through data
      For R = 0 To ptIndex.Count - 1
        With Dat.Rows(R)
          .Cells(0).Value = R
          .Cells(1).Value = ptIndex(R).ShpID
          .Cells(2).Value = ptIndex(R).PartNum
          .Cells(3).Value = ptIndex(R).PtID
          .Cells(4).Value = ptIndex(R).nextID
          .Cells(5).Value = ptIndex(R).prevID
        End With
        ' report progress
        If (R + 1) Mod updateIncrement = 0 Then If Not P Is Nothing Then P.setCompleted(R + 1)
      Next
      ' report finish
      If Not P Is Nothing Then P.finishTask()
    End Sub
    'Shared Function numPointsInShapefile(ByVal SF As Shapefile) As Integer
    '  Dim R As Integer = 0
    '  Dim curShp As Integer
    '  For curShp = 0 To SF.NumShapes - 1
    '    R += SF.numPoints(curShp)
    '  Next
    '  Return R
    'End Function
    Shared Function pointInPoly(ByVal Pt As DotSpatial.Topology.Coordinate, _
                                ByVal Poly As DotSpatial.Data.Feature) As Boolean
      ' returns true if C is inside P (or on edge)
      ' false otherwise
      Dim P As DotSpatial.Topology.Coordinate = Pt
      Dim V() As DotSpatial.Topology.Coordinate = Poly.Coordinates.ToArray
      Dim V1, V2 As DotSpatial.Topology.Coordinate
      Dim R As Boolean = False
      For i = 0 To V.Length - 1
        V1 = V(i)
        If i = V.Length - 1 Then V2 = V(0) Else V2 = V(i + 1)
        If (V1.X < P.X) <> (V2.X < P.X) Then
          If P.Y < V1.Y + (V2.Y - V1.Y) * (P.X - V1.X) / (V2.X - V1.X) Then
            R = Not R
          End If
        End If
      Next
      Return R
    End Function
  End Class
  'Public Class Geometry
  '  Overloads Shared Function Distance(ByVal pt1 As MapWinGIS.Point, ByVal pt2 As MapWinGIS.Point) As Double
  '    Return ((pt1.x - pt2.x) ^ 2 + (pt1.y - pt2.y) ^ 2) ^ 0.5
  '  End Function
  '  Overloads Shared Function distance(ByVal pt1 As MapWinGIS.Point, ByVal x2 As Double, ByVal y2 As Double) As Double
  '    Return ((pt1.x - x2) ^ 2 + (pt1.y - y2) ^ 2) ^ 0.5
  '  End Function
  '  Overloads Shared Function distance(ByVal x1 As Double, ByVal y1 As Double, ByVal x2 As Double, ByVal y2 As Double) As Double
  '    Return ((x1 - x2) ^ 2 + (y1 - y2) ^ 2) ^ 0.5
  '  End Function
  'End Class
End Namespace
Public Class StatisticsCalculator
  Dim EXC As Microsoft.Office.Interop.Excel.Application
  Dim workFunc As Microsoft.Office.Interop.Excel.WorksheetFunction
  Public Sub Open()
    ' must be called before using the object
    EXC = New Microsoft.Office.Interop.Excel.Application
    workFunc = EXC.WorksheetFunction
  End Sub
  Public Sub Close()
    ' please call this when done using the object
    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(EXC)
    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(workFunc)

  End Sub
#Region "Mann Whitney U"
  ' calculates the Mann-Whitney U test statistic, z-score and significance
  ' based on Wikipedia
  ' does not look for or account for ties
  Shared Function MannWhitneyU(ByVal V1() As Double, _
                               ByVal V2() As Double) _
                             As Long
    ' calculates the U statistic
    ' based on Wikipedia
    ' doesn't calculate significance

    ' get counts
    Dim N1 As Integer = V1.Length
    Dim N2 As Integer = V2.Length
    ' sort objects (I'm assuming this is low to high)
    System.Array.Sort(V1)
    System.Array.Sort(V2)
    Dim Rank1 As Integer = 0
    Dim Rank2 As Integer = 0
    Dim CombinedRank As Integer = 0
    ' calculates sum of ranks for array 1
    Dim V1RankSum As Integer = 0
    ' go through both arrays together, keeping the smallest unhandled
    ' value in each
    Do
      CombinedRank += 1
      If V1(Rank1) < V2(Rank2) Then
        ' V1 is smaller, so it's rank is determined
        V1RankSum += CombinedRank
        Rank1 += 1
      Else
        Rank2 += 1
      End If
    Loop Until Rank1 = N1 Or Rank2 = N2
    ' if highest v2 is lower than highest v1, keep going to
    ' handle remaining v1 values
    For i = Rank1 To N1 - 1
      CombinedRank += 1
      V1RankSum += CombinedRank
    Next
    ' calculate U
    Dim U As Long
    U = V1RankSum - N1 * (N2 + 1) / 2
    Return U
  End Function
  Shared Function MannWhitneyU_mean(ByVal N1 As Integer, ByVal N2 As Integer) As Double
    Return N1 * N2 / 2
  End Function
  Shared Function MannWhitneyU_stdev(ByVal N1 As Integer, ByVal N2 As Integer)
    Return Math.Sqrt(N1 * N2 * (N1 + N2 + 1) / 12)
  End Function
  Shared Function MannWhitneyZScore(ByVal V1() As Double, ByVal V2() As Double) As Double
    ' returns the Z-score of the Mann-Whitney U test
    ' should be used only for sample sizes > 20
    Return (MannWhitneyU(V1, V2) - MannWhitneyU_mean(V1.Length, V2.Length)) / MannWhitneyU_stdev(V1.Length, V2.Length)
  End Function
  Public Function test() As Integer
    ' computes U for the Wikipedia example
    Dim V1() As Double = {1, 7, 8, 9, 10, 11}
    Dim V2() As Double = {2, 3, 4, 5, 6, 12}
    Dim U As Double = Me.MannWhitneyU(V1, V2)
    Return U
  End Function
#End Region
  Shared Function stdevPop(ByVal Values() As Double, ByVal mean As Double) As Double
    ' returns the population standard deviation
    Dim R As Double = 0
    For Each v In Values
      R += (v - mean) ^ 2
    Next v
    R = R / Values.Length
    Return R
  End Function
#Region "T-test"
  Shared Function tStat_separate(ByVal V1() As Double, ByVal V2() As Double) As Double
    ' returns the student t test statistic using separate variances
    Dim n1 As Double = V1.Length
    Dim n2 As Double = V2.Length
    Dim mu1 As Double = V1.Average
    Dim mu2 As Double = V2.Average
    Dim s1 As Double = stdevPop(V1, mu1)
    Dim s2 As Double = stdevPop(V2, mu2)
    Dim denominator As Double = Math.Sqrt((s1 ^ 2) / n1 + (s2 ^ 2) / n2)
    Dim R As Double = (mu1 - mu2) / denominator
    Return R
  End Function
  Public Function Tsig_separate_twoTail(ByVal t As Double, _
                                        ByVal N1 As Integer, _
                                        ByVal N2 As Integer) As Double
    ' returns the 2-tailed significance (p) value
    ' for a t-test with separate variances
    Dim degreesOfFreedom As Integer = N1 + N2 - 2
    ' excel function only works on positive t-values (distribution is symmetrical)
    If t < 0 Then t = -t
    Return workFunc.T_Dist_2T(t, degreesOfFreedom)
  End Function
#End Region
  
#Region "Z-scores"
  Public Function Zsig_oneTail(ByVal z As Double) As Double
    ' uses excel sheet (please open and close)
    Return workFunc.Norm_S_Dist(z, True)
  End Function
#End Region



End Class
Namespace Display
  Public Class Colors
    Shared Function randomMWColor() As UInt32
      Dim R, G, B As Integer
      Randomize()
      R = Int(Rnd() * 255)
      G = Int(Rnd() * 255)
      B = Int(Rnd() * 255)
      Return System.Convert.ToUInt32(RGB(R, G, B))
    End Function
    Shared Function brewerColor8(ByVal brewerNumber As Integer) As UInt32
      Select Case brewerNumber Mod 8
        Case Is = 0
          Return System.Convert.ToUInt32(RGB(27, 158, 119))
        Case Is = 1
          Return System.Convert.ToUInt32(RGB(217, 95, 2))
        Case Is = 2
          Return System.Convert.ToUInt32(RGB(117, 112, 179))
        Case Is = 3
          Return System.Convert.ToUInt32(RGB(231, 41, 138))
        Case Is = 4
          Return System.Convert.ToUInt32(RGB(102, 166, 30))
        Case Is = 5
          Return System.Convert.ToUInt32(RGB(230, 171, 2))
        Case Is = 6
          Return System.Convert.ToUInt32(RGB(166, 118, 29))
        Case Is = 7
          Return System.Convert.ToUInt32(RGB(102, 102, 102))
      End Select
    End Function
    Shared Function MWColor(ByVal R As Byte, ByVal G As Byte, ByVal B As Byte) As UInt32
      Return System.Convert.ToUInt32(RGB(R, G, B))
    End Function
  End Class
End Namespace
Namespace misc
  Public Class formatting
    Public Shared Function numToText(ByVal num As Double, ByVal numDecimalPlaces As Integer) As String
      Dim R As String
      If numDecimalPlaces < 0 Then
        R = num.ToString
      Else
        R = num.ToString("F" & numDecimalPlaces.ToString)
      End If
      Return R
    End Function
    Public Overloads Shared Function toFixedWidthString(inStr As String, width As Integer, Optional sepChr As String = " ", Optional numDecimals As Integer = 3) As String
      ' creates a string of the desired width
      If inStr.Length < width Then
        Return inStr.PadRight(width - 1) & sepChr
      Else
        Return inStr.Substring(0, width - 1) & sepChr
      End If
    End Function
    Public Overloads Shared Function toFixedWidthString(inVal As Double, width As Integer, Optional sepChr As String = " ") As String
      ' creates a string of the desired width from the input value
      ' if this cannot be done, returns string of #####...
      Dim valStr As String = CStr(inVal)
      If valStr.Length < width - 1 Then
        Return toFixedWidthString(valStr, width)
      ElseIf valStr.Substring(width - 2) = "." Then
        Return valStr.Substring(0, width - 2) & " " & sepChr
      ElseIf CStr(Int(inVal)).Length >= width Then
        Return Strings.StrDup(width - 1, "#") & sepChr
      Else
        Return toFixedWidthString(valStr, width)
      End If
    End Function
  End Class
  Public Class enumUtils
    Public Shared Function enumNames(enumType As Type) As List(Of String)
      Dim items As Array
      items = System.Enum.GetNames(enumType)
      Dim R As New List(Of String)
      For Each item In items
        R.Add(item)
      Next
      Return R
    End Function
    Public Shared Function enumValue(enumType As Type, enumName As String) As Object
      Return [Enum].Parse(enumType, enumName)
    End Function


  End Class
End Namespace