unit Person;

interface

uses
  SysUtils, Windows, Messages, Classes, Graphics, Controls,
  Forms, Dialogs, GameTalk;

type
  IPerson = interface (IInterface)
    function GetAge: Integer;
    function GetName: string;
    procedure SetAge(Value: Integer);
    procedure SetName(const Value: string);
    property Age: Integer read GetAge write SetAge;
    property Name: string read GetName write SetName;
  end;
  
  TPerson = class (TEntity, IPerson)
  private
    FAge: Integer;
    FName: string;
    function GetAge: Integer;
    function GetName: string;
    procedure SetAge(Value: Integer);
    procedure SetName(const Value: string);
  public
    property Age: Integer read GetAge write SetAge;
    property Name: string read GetName write SetName;
  end;
  
  TPersonSvr = class (TPerson, IEntitySvr)
  public
    procedure Changed(FieldID: Integer; Value: TObject);
    function Serialise: TPacket;
  end;
  
  TPersonCln = class (TPerson, IEntityCln)
  public
    procedure Deserialise(Data: TPacket);
    procedure Update(FieldID: Integer; Value: TObject);
  end;
  
  TPersonListCln = class (TEntityListCln)
  private
    function GetItems(Index: Integer): TPersonCln;
  public
    procedure Deserialise(Data: TPacket); override;
    property Items[Index: Integer]: TPersonCln read GetItems; default;
  end;
  
  TPersonListSvr = class (TEntityListSvr)
  private
    function GetItems(Index: Integer): TPersonSvr;
  public
    procedure Changed(FieldID: Integer; Value: TObject); override;
    function Serialise: TPacket; override;
    property Items[Index: Integer]: TPersonSvr read GetItems; default;
  end;
  

implementation

{ TPerson }

function TPerson.GetAge: Integer;
begin
  Result := FAge;
end;

function TPerson.GetName: string;
begin
  Result := FName;
end;

procedure TPerson.SetAge(Value: Integer);
begin
  FAge := Value;
end;

procedure TPerson.SetName(const Value: string);
begin
  FName := Value;
end;

{ TPersonSvr }

procedure TPersonSvr.Changed(FieldID: Integer; Value: TObject);
begin
  ChangeLog.Add(FieldID, Value);
end;

function TPersonSvr.Serialise: TPacket;
begin
  Result := TPacket.Create;
  Result.WriteInteger(Age);
  Result.WriteString(Name);
end;

{ TPersonCln }

procedure TPersonCln.Deserialise(Data: TPacket);
begin
  Age := Data.ReadInteger;
  Name := Data.ReadString;
end;

procedure TPersonCln.Update(FieldID: Integer; Value: TObject);
begin
  case FieldID of
    0: Age := Integer(Value);
    1: Name := String(Value);
  else
    Assert(True, 'Invalid FieldID');
  end;
end;

{ TPersonListCln }

procedure TPersonListCln.Deserialise(Data: TPacket);
var
  iCount: Integer;
  iID: Integer;
begin
  for iCount := 0 to Data.ReadInteger do
  begin
    iID := Data.ReadInteger;
    // Self.Add(DataPool.FindEntity(iID));
  end;
end;

function TPersonListCln.GetItems(Index: Integer): TPersonCln;
begin
  Result := TPersonCln(Items[Index]);
end;

{ TPersonListSvr }

procedure TPersonListSvr.Changed(FieldID: Integer; Value: TObject);
begin
end;

function TPersonListSvr.GetItems(Index: Integer): TPersonSvr;
begin
  Result := TPersonSvr(Items[Index]);
end;

function TPersonListSvr.Serialise: TPacket;
var
  iIndex: Integer;
begin
  Result := TPacket.Create;
  Result.WriteInteger(Self.Count);
  
  for iIndex := 0 to Self.Count - 1 do
    Result.WriteInteger(Self[iIndex].ID);
end;


end.
