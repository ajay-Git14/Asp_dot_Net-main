# Manual Test Checklist

Run the project and mark each result.

| # | Request | Expected result | Pass |
|---|---|---|---|
| 1 | `/` | Home page opens | ☐ |
| 2 | `/complaints/1` | Complaint 1 details | ☐ |
| 3 | `/complaints/status/Open` | Open complaint list | ☐ |
| 4 | `/RoutingDemo/Optional` | id not supplied | ☐ |
| 5 | `/RoutingDemo/Optional/2` | id = 2 | ☐ |
| 6 | `/track` | Attribute route result | ☐ |
| 7 | `/track/CMP-2026-0001` | Complaint details | ☐ |
| 8 | `/RoutingDemo/TokenDemo/7` | Route tokens result | ☐ |
| 9 | `/verb-demo/2` | GET template result | ☐ |
| 10 | Home POST form | POST `/verb-demo` result | ☐ |
| 11 | `/pages` | pageNumber = 1 | ☐ |
| 12 | `/pages/4` | pageNumber = 4 | ☐ |
| 13 | `/category` | All categories | ☐ |
| 14 | `/category/Maintenance` | Category value shown | ☐ |
| 15 | `/building/A/floor/3` | block A, floor 3 | ☐ |
| 16 | `/society/complaints/2` | combined route result redirects to complaint 2 | ☐ |
| 17 | `/reports/latest` | literal route selected | ☐ |
| 18 | `/reports/2026` | constrained parameter route selected | ☐ |
| 19 | `/search?term=lift&page=2` | query values shown | ☐ |
| 20 | `/go/list` | redirects to Index | ☐ |
| 21 | `/go/complaint/3` | redirects to complaint 3 | ☐ |
| 22 | `/Admin/Dashboard` | area value is Admin | ☐ |
| 23 | Create valid complaint | redirects to friendly details route | ☐ |
| 24 | `/complaints/abc` | 404 because `int` constraint fails | ☐ |
| 25 | `/track/ABCDE` | 400 because business validation fails | ☐ |
