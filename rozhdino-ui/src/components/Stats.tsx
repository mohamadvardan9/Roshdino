{
  /*بخش زیر Hero*/
}
{
  /*این بخش برای نمایش وضعیت سایت است*/
}

import Container from "./ui/Container";

const stats = [
  {
    title: "+50",
    text: "پروژه موفق",
  },
  {
    title: "+30",
    text: "مشتری",
  },
  {
    title: "+5",
    text: "خدمت تخصصی",
  },
  {
    title: "98%",
    text: "رضایت",
  },
];

export default function Stats() {
  return (
    <section
      className="
py-1
-mt-9
"
    >
      <Container>
        <div
          className="
grid grid-cols-2 md:grid-cols-4
gap-6
px-6
"
        >
          {stats.map((item) => (
            <div
              key={item.title}
              className="
rounded-2xl
border
p-8
text-center
hover:shadow-xl
transition
"
            >
              <h3
                className="
text-4xl
font-bold
text-purple-600
"
              >
                {item.title}
              </h3>

              <p
                className="
mt-3
text-gray-500
"
              >
                {item.text}
              </p>
            </div>
          ))}
        </div>
      </Container>
    </section>
  );
}
