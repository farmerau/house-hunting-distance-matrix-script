namespace Austin.Farmer.Codes.HouseHunting.DistanceMatrix;

public record GoogleMapsDistanceMatrixResponse(GoogleMapsDistanceMatrixRow[] Rows);

public record GoogleMapsDistanceMatrixRow(GoogleMapsDistanceMatrixRowElement[] Elements);


public record GoogleMapsDistanceMatrixRowElement(GoogleMapsDistanceMatrixRowElementValue Distance, GoogleMapsDistanceMatrixRowElementValue Duration);

public record GoogleMapsDistanceMatrixRowElementValue(string Text, long Value);

