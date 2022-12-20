using User;

//@Entity @Data @NoArgsConstructor @AllArgsConstructor
public class School {

    //@Id
    //@GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    //@Column(unique=true)
    private String name;

    //@OneToMany(mappedBy="school")
    private List<User> user = new List<User>();
}