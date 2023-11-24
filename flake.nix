{
  description = "cloud-run-asp-net-http2";

  inputs = {
    nixpkgs.url = "github:nixos/nixpkgs/nixpkgs-unstable";
    flake-utils.url = "github:numtide/flake-utils";
  };

  outputs = { self, nixpkgs, flake-utils, ... }@inputs:
    let
      overlays = [
        # none, yet
      ];
    in
    flake-utils.lib.eachDefaultSystem (system:
      let
        pkgs = import nixpkgs { inherit system overlays; };
      in
      rec {
        devShell = pkgs.mkShell rec {
          name = "cloud-run-asp-net-http2";

          buildInputs = with pkgs; [
            dotnet-sdk_8
            google-cloud-sdk
          ];
        };
      }
    );
}
