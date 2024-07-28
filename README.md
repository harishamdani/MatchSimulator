# How to Run MatchSimulator

## Prerequisites

1. **Visual Studio 2022**
2. **.NET 8.0**

## Steps to Run Locally via Visual Studio 2022

1. Open Visual Studio 2022.
2. Load the `web-api` project in Visual Studio.
3. Start the project to run locally.

## Using the App

1. Enter the number of teams you want to simulate and the number of qualified teams in the group.
2. For example, calling `SimulateTournament(4,2)` will:
    - Generate 4 teams.
    - Simulate the tournament.
    - Return 2 teams qualified to the next round from this group.
